using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace core.Util
{
    public class Constantes
    {

        public static int Porcentagem(decimal meta, decimal valor)
        {
            if (meta == 0)
                meta = 1;

            return Convert.ToInt32(Math.Round(((valor / meta) * 100),0));
        }

        public static class TipoMsgSistema
        {
            public const string Informativo = "Informativo";
            public const string NovaCotacao = "NovaCotacao";
            public const string Alerta = "Alerta";
        }

        public static class TipoLogin
        {
            public const string Buyer = "1d367f51-c965-4c29-8aab-070d915230ac";
            public const string Seller = "37800c82-65e9-4138-ab35-268ee68ea165";
            public const string PontoVenda = "dac39b43-415a-4af2-ae9e-bcdf0bf116b0";
        }

        public static class CorIndicador
        {
            public const decimal otimo = 100;
            public const decimal medio = 70;
            public const decimal ruim = 50;
        }

      
        public static class TipoUsuario
        {
            public const int Captador = 1;
            public const int CoordenadorCaptador = 2;
            public const int TeleVendedor = 3;
            public const int CoordenadorTeleVendedor = 4;
            public const int Gerente = 5;

        }

        public static class TituloMsgPadraoSistema
        {
            public const string ProdutoAceito = "Sugestão de produto.";
            public const string ProdutoNegado = "Sugestão de produto, negada.";
        }

        public static class MsgPadraoSistema
        {
            public const string ProdutoAceito = "A sua sugestão foi aceita, o produto já está disponivel no portifólio matriz.";
            public const string ProdutoNegado = "A sua sugestão não foi aceita.";
            public const string ErroAPI = "Oops parece que estamos enfrentando algum problema T_T. Tente novamente mais tarde. Se o erro persistir contate nosso suporte suporte@mostazapago.com.br";
        }

        public static class Chaves
        {

            public const string ChaveApi = "";
            public const string Criptografia = "";

            public const string ChaveApiProd = "";
            public const string CriptografiaProd = "";

        }

        public static class StatusTransacao
        {
            public const int Aberta = 0;
            public const int Paga = 1;
            public const int Estornada = 2;
            public const int AguardandoBuyer = 3;
            public const int NaoFaturada = 4;
            public const int Cancelada = 99;
        }

        public static class StatusBuyer
        {
            public const int Inativo = 0;
            public const int Ativo = 1;
            public const int AguardandoAprovacao = 3;
            public const int Inadimplente = 4;
        }

        public static class StatusFatura
        {
            public const int Aberta = 0;
            public const int Paga = 1;
            public const int Cancelada = 99;
        }

        public static class ParametrosGerais
        {
            public const int DiaVencimento = 10;
        }

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static string AlfanumericoAleatorio(int tamanho)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, tamanho)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public static string LimparTexto(string str)
        {
            str = str.Replace("=", "");
            str = str.Replace("'", "");
            str = str.Replace("\"", "");
            str = str.Replace(" or ", "");
            str = str.Replace(" and ", "");
            str = str.Replace("(", "");
            str = str.Replace(");", "");
            str = str.Replace("<", "[");
            str = str.Replace(">", "]");
            str = str.Replace("update", "");
            str = str.Replace("-shutdown", "");
            str = str.Replace("--", "");
            str = str.Replace("'", "");
            str = str.Replace("#", "");
            str = str.Replace("$", "");
            str = str.Replace("%", "");
            str = str.Replace("¨", "");
            str = str.Replace("&", "");
            str = str.Replace("'or'1'='1'", "");
            str = str.Replace("--", "");
            str = str.Replace("insert", "");
            str = str.Replace("drop", "");
            str = str.Replace("delet", "");
            str = str.Replace("xp_", "");
            str = str.Replace("select", "");
            str = str.Replace("*", "");
            return str;
        }

        public static string MetodosSistema()
        {
            string actions = $@"[
            {{
                'Action': 'ConsultaBuyer',
                'Categoria': '{TipoLogin.PontoVenda}'
            }},
            {{
                'Action': 'ConsultaBuyerFiltro',
                'Categoria': '{TipoLogin.PontoVenda}'
             }},
            {{
                'Action': 'ConsultaBuyer',
                'Categoria': '{TipoLogin.Seller}'
            }},
            {{
                'Action': 'ConsultaBuyerFiltro',
                'Categoria': '{TipoLogin.Seller}'
             }},
            {{
                'Action': 'AlterarBuyer',
                'Categoria': '{TipoLogin.Buyer}'
             }},
            {{
                'Action': 'AlterarBuyerSenha',
                'Categoria': '{TipoLogin.Buyer}'
             }},
            {{
                'Action': 'AlterarBuyerStatus',
                'Categoria': '{TipoLogin.PontoVenda}'
             }},
            {{
                'Action': 'AlterarBuyerStatus',
                'Categoria': '{TipoLogin.Seller}'
             }}
        ]";

            return actions.Replace("'", "\"");
        }
    }
}
