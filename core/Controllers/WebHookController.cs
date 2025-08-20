using core.Domain.Entities;
using core.Domain.Entities.Models;
using core.Service;
using log4net.Config;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebHookController : ControllerBase
    {
        private readonly WebHookService _webhookService;
        private readonly IConfiguration _configuration;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public WebHookController(WebHookService webhookService, IConfiguration configuration)
        {
            _webhookService = webhookService;
            _configuration = configuration;

        }

        [HttpGet(Name = "WebHook")]
        public IActionResult VerifyWebhook(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] int challenge,
            [FromQuery(Name = "hub.verify_token")] string verifyToken)
        {
            var expectedVerifyToken = _configuration["Webhooks:VerifyToken"];
            if (mode == "subscribe" && verifyToken == expectedVerifyToken)
            {
                return Ok(challenge);
            }

            return BadRequest("Invalid verification request.");
        }

        [HttpPost(Name = "WebHook")]
        public async Task<IActionResult> SaveMessages()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string jsonData = await reader.ReadToEndAsync();
                    var data = JsonConvert.DeserializeObject<RootObject>(jsonData);
                    var responseData = JsonConvert.DeserializeObject<WebHook>(jsonData);

                    _log.Info(jsonData);
                    _log.Info(responseData);

                    var firstChange = data.Entry[0].Changes[0];


                    if (firstChange.Value.Statuses == null)
                    {

                        _log.Info("Message saved successfully");
                        _webhookService.SaveMessage(responseData);
                    }else if (jsonData.Contains("Spam Rate limit hit"))
                    {
                        _log.Info("Spam alcançado");
                        _webhookService.SaveMessageSpam(responseData);
                    }
                    else
                    {
                        _log.Info("Status changed");
                        _webhookService.SaveMessageStatus(responseData);
                    }
                }

                return Ok();
            }catch (Exception ex)
            {
                _log.Error("An error occurred: {ErrorMessage}", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
