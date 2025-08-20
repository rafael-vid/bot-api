using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using log4net;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using core.Domain.Entities.MetaWpp;
using System;
using core.Service;
using System.Threading.Tasks;

namespace core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetaWppController : Controller
    {
        private readonly IConfiguration _configuration;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly IMetaWppService _MetaWppService;


        public MetaWppController(IConfiguration configuration, IMetaWppService metaWppService)
        {
            _configuration = configuration;
            _MetaWppService = metaWppService;
        }

        [Authorize]
        [HttpPost]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest messageRequest)
        {
            try
            {
                return new JsonResult(await _MetaWppService.SendMessageMetaAsync(messageRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
