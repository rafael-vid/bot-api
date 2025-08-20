using core.Domain.Entities;
using core.Service;
using MySql.Data.MySqlClient;
using System;
using System.Threading;
using System.Text.Json;

namespace core.Util
{
    public class chatbot
    {
        public class MensagemRetorno
        {
            public string Texto { get; set; }
            public string Numero { get; set; }
            public int midia { get; set; }
            public string url { get; set; }
            public string imagemName { get; set; }
        }
        public string ProcessaMensagem(string telefone, string mensagem)
        {
            FornecedorService fornecedor = new FornecedorService();
            //var delay = fornecedor.GetDelay();
            var fornecedorRetorno = fornecedor.GetFornecedor();
            foreach (var forn in fornecedorRetorno)
            {
                //Thread.Sleep(delay);
                if (telefone == forn.Numero_Telefone)
                {
                    string estado = forn.estado;

                    Console.WriteLine($"Telefone encontrado: {telefone}, Estado: {estado}");

                    if (string.IsNullOrEmpty(estado))
                    {
                        AtualizarEstado(fornecedor, telefone, "1");
                        var msg = BuscarMensagemRetorno(fornecedor, "8", 1);
                        return SerializarMensagem(msg);
                    }
                    if (estado == "10")
                    {
                        AtualizarEstado(fornecedor, telefone, "1");
                        var msg = BuscarMensagemRetorno(fornecedor, "18", 1);
                        return SerializarMensagem(msg);
                    }
                    int bloco = int.Parse(estado) + 9;
                    int numero = 0;
                    try
                    {
                        numero = int.Parse(mensagem);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Mensagem não é um número válido.");
                        var msg = BuscarMensagemRetorno(fornecedor, "9", 1);
                        return SerializarMensagem(msg);
                    }
                    MensagemRetorno retornoMensagem;
                    try
                    {
                        retornoMensagem = BuscarMensagemRetorno(fornecedor, bloco.ToString(), numero);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Mensagem não é um número válido.");
                        retornoMensagem = BuscarMensagemRetorno(fornecedor, "9", 1);
                        return SerializarMensagem(retornoMensagem);
                    }
                    string proximoEstado = retornoMensagem.Numero;
                    AtualizarEstado(fornecedor, telefone, proximoEstado);
                    return SerializarMensagem(retornoMensagem);
                }
            }
            string mensagemRetorno = string.Empty;
            Console.WriteLine("Nenhum telefone correspondente encontrado.");
            return mensagemRetorno;
        }

        private void AtualizarEstado(FornecedorService fornecedor, string telefone, string novoEstado)
        {
            if (!string.IsNullOrEmpty(novoEstado))
            {
                fornecedor.AtualizarEstado(telefone, novoEstado);
            }

        }

        private MensagemRetorno BuscarMensagemRetorno(FornecedorService fornecedor, string bloco, int numero)
        {
            BuscarMensagem mensagem = fornecedor.BuscarMensagem(bloco, numero);
            if (mensagem != null)
            {
                return new MensagemRetorno
                {
                    Texto = mensagem.mensagem,
                    Numero = mensagem.proximoEstado,
                    midia = mensagem.midia ?? 1,
                    url = mensagem.url
                };
            }
            return null; // Return null if the message is not found
        }
        private string SerializarMensagem(MensagemRetorno msg)
        {
            if (msg == null) return string.Empty;

            // For media types (2 = image, 3 = video) return a JSON payload
            if (msg.midia == 2 || msg.midia == 3)
            {
                Console.WriteLine($"Enviando mídia com URL: {msg.url}");
                return JsonSerializer.Serialize(new
                {
                    midia = msg.midia,
                    mensagem = msg.Texto,
                    url = msg.url,
                    imagemName = msg.imagemName
                });
            }

            // Default behaviour for plain text messages
            return msg.Texto ?? string.Empty;
        }
    }
}
