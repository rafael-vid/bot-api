using Comtele.Sdk.Services;


namespace core.Util
{
    public class Sms
    {

        public bool SmsSend(string telefone, string msg)
        {
            Settings app = new Settings();
            string API_KEY = app.Appsettings("SmsKey");
            var textMessageService = new TextMessageService(API_KEY);
            var result = textMessageService.Send("", msg, new string[] { telefone });

            return result.Success;
        }

    }
}
