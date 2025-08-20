using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.Util
{
    using System.Net;
    using System.Net.Mail;
    public class Email
    {

        public string EnviaEmail(string html, string assunto, string destinatario)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.umbler.com", 587);
                smtpClient.EnableSsl = false;
                MailMessage message = new MailMessage(new MailAddress("impulsecapital@movimentacaofinanceira.com.br", assunto), new MailAddress("dias@diasadm.com", assunto));
                message.IsBodyHtml = true;
                string str = html;
                message.Body = str;
                message.Subject = assunto;
                NetworkCredential networkCredential = new NetworkCredential("impulsecapital@movimentacaofinanceira.com.br", "impulsecapital@2022", "");
                smtpClient.Credentials = (ICredentialsByHost)networkCredential;
                Console.WriteLine("Enviando...");

                smtpClient.Send(message);

                return "OK-" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:s");

            }
            catch (Exception ex)
            {
                return "ERRO -" + ex.Message.ToString().Replace("'", "") + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:s");
            }
        }

        public string EnvioExtratoViver(string nome, string produto, string nomeProduto, string cliente)
        {
            Settings app = new Settings();

            return @"<!DOCTYPE html>
<html lang='en'>
  <head>
    <meta charset='UTF-8' />
    <meta http-equiv='X-UA-Compatible' content='IE=edge' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Impulse</title>
    <link rel='preconnect' href='https://fonts.googleapis.com' />
    <link rel='preconnect' href='https://fonts.gstatic.com' crossorigin />
    <link
      href='https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;700&display=swap'
      rel='stylesheet'
    />
  </head>
  <body style='margin: 0; padding: 0; box-sizing: border-box; font-family: 'Roboto', sans-serif; height: 100vh;'>
    <table style='background-color:#fbd961; width: 100%; margin: 0 auto; padding: 30px 0;'>
        <tbody>
            <tr>
                <td style='background-color:#fbd961; width: 90%; margin: 0 auto; padding: 30px 20px'>
                    <h2>Impulse Captal</h2>
                    <h1>Olá " + nome + @",</h1>
                    <h2>Temos ótimas noticias.</h2>
                    <p>&nbsp;</p>
                    <span>O seu demonstrativo de rendimentos para o produto " + nomeProduto + @" já está disponível para consulta.</span>
                    <p>&nbsp;</p>
                    <a href='http://189.18.149.66:5312/Painel/placeholder?c=" + cliente + @"' style='padding:15px 60px; max-width: 300px; width: 100%; background: #000; color: #fff; text-decoration: none; font-size: 18px; height: 50px; margin-top: 50px; text-transform: uppercase; letter-spacing: 1px; border-radius: 4px;'>Acessar Extrato</a>
                </td>
            </tr>
        </tbody>
    </table>
    <footer style='padding: 15px'>
        <span style='display: block; text-align: center'>Impulse " + DateTime.Now.Year + @"</span>
    </footer>
</body>
</html>";
        }

        public string EnvioExtrato(string nome, string produto, string nomeProduto, string cliente)
        {
            Settings app = new Settings();

            return @"<!DOCTYPE html>
<html lang='en'>
  <head>
    <meta charset='UTF-8' />
    <meta http-equiv='X-UA-Compatible' content='IE=edge' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Impulse</title>
    <link rel='preconnect' href='https://fonts.googleapis.com' />
    <link rel='preconnect' href='https://fonts.gstatic.com' crossorigin />
    <link
      href='https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;700&display=swap'
      rel='stylesheet'
    />
  </head>
  <body style='margin: 0; padding: 0; box-sizing: border-box; font-family: 'Roboto', sans-serif; height: 100vh;'>
    <table style='background-color:#fbd961; width: 100%; margin: 0 auto; padding: 30px 0;'>
        <tbody>
            <tr>
                <td style='background-color:#fbd961; width: 90%; margin: 0 auto; padding: 30px 20px'>
                    <h2>Impulse Captal</h2>
                    <h1>Olá " + nome + @",</h1>
                    <h2>Temos ótimas noticias.</h2>
                    <p>&nbsp;</p>
                    <span>O seu demonstrativo de rendimentos para o produto " + nomeProduto + @" já está disponível para consulta.</span>
                    <p>&nbsp;</p>
                    <a href='http://189.18.149.66:5312/Painel/DetalheExtratoCliente?c=" + cliente + @"' style='padding:15px 60px; max-width: 300px; width: 100%; background: #000; color: #fff; text-decoration: none; font-size: 18px; height: 50px; margin-top: 50px; text-transform: uppercase; letter-spacing: 1px; border-radius: 4px;'>Acessar Extrato</a>
                </td>
            </tr>
        </tbody>
    </table>
    <footer style='padding: 15px'>
        <span style='display: block; text-align: center'>Impulse " + DateTime.Now.Year + @"</span>
    </footer>
</body>
</html>";
        }

    }
}
