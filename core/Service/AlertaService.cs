using System.Collections.Generic;
using System.IO;


namespace core.Service
{
    using System;
    using core.Domain.Entities;
    using core.Domain.Interfaces;
    using core.Service;

    public class AlertaService : IAlertaService
    {

        private readonly IAlertaRepository _AlertaRepository;

        public AlertaService(IAlertaRepository AlertaRepository)
        {
            _AlertaRepository = AlertaRepository;
        }

        public List<alerta> GetFila()
        {
            _AlertaRepository.SalvaLog();
            var ret = _AlertaRepository.GetFila();

            foreach (var item in ret)
            {
                item.telefone = item.telefone.Replace("+", "");

                if (item.telefone.Substring(0, 2) != "55")
                {
                    item.telefone = "55" + item.telefone;
                }
            }
            return ret;
        }

        public List<alerta> GetFilaRespostaUnica()
        {
            _AlertaRepository.SalvaLog();
            List<alerta> ret = _AlertaRepository.GetFilaRespostaUnica();

            foreach (var item in ret)
            {
                item.telefone = item.telefone.Replace("+", "");

                if (item.telefone.Substring(0, 2) != "55")
                {
                    item.telefone = "55" + item.telefone;
                }
            }
            return ret;
        }

        public alerta GetUltimaMensagem(string telefone)
        {
            _AlertaRepository.SalvaLog();
            alerta ret = _AlertaRepository.GetUltimaMensagem(telefone);

            ret.telefone = ret.telefone.Replace("+", "");

            if (ret.telefone.Substring(0, 2) != "55")
            {
                ret.telefone = "55" + ret.telefone;
            }
            return ret;
        }

        public string InserirMensagem(alertaInsert mensagem, string grupo)
        {
            if (mensagem == null) throw new ArgumentNullException(nameof(mensagem));

            // Default/normalize midia (1=text, 2=image)
            var midia = mensagem.midia ?? 1;
            if (midia != 1 && midia != 2 && midia != 3) midia = 1;

            // Basic message text safety
            mensagem.mensagem ??= string.Empty;

            // If it's an image, validate URL and derive filename when missing
            if (midia == 2 || midia == 3)
            {
                if (string.IsNullOrWhiteSpace(mensagem.url))
                    throw new ArgumentException("url é obrigatório quando midia = 2 ou 3.", nameof(mensagem.url));

                if (!Uri.TryCreate(mensagem.url, UriKind.Absolute, out var uri))
                    throw new ArgumentException("url inválido. Informe uma URL absoluta (http/https).", nameof(mensagem.url));
            }
            else
            {
                // Not an image → clear image fields to avoid accidental persistence
                mensagem.url = null;
                mensagem.imagemName = null;
            }

            // Persist normalized midia back into the DTO
            mensagem.midia = midia;

            // Generate ID and persist
            string id = Guid.NewGuid().ToString();

            // If your DB stores 'grupo' in a separate column, thread it into the repository signature.
            // For now we keep the existing signature to avoid breaking changes.
            _AlertaRepository.InserirMensagem(mensagem, id);

            return id;
        }


        public void SalvaLog(string telefone, string mensagem)
        {
            _AlertaRepository.SalvaLogDebug(telefone, mensagem);
        }

        public void UpdateMensagem(string id)
        {
            _AlertaRepository.UpdateMensagem(id);
        }

        public void UpdateMensagemRecebida(string id, string resposta, DateTime dataresposta)
        {  
            _AlertaRepository.UpdateMensagemRecebida(id, resposta,dataresposta);
        }
    }
}
