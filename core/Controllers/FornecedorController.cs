using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Web.Http;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.AspNetCore.Hosting;

namespace core.Controllers
{
    using ClosedXML.Excel;
    using core.Domain.Entities;
    using core.Domain.Interfaces;
    using core.Service;
    using DocumentFormat.OpenXml.Drawing;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : Controller
    {
        //protected readonly IFornecedorService _FornecedorService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _config;
        int take = 100;
        public FornecedorController(IWebHostEnvironment webHostEnvironment, IConfiguration Configuration, ICredencialService CredencialService)
        {
            _webHostEnvironment = webHostEnvironment;
            //_FornecedorService = fornecedorService;

            _config = Configuration;
        }

        
        
        [HttpGet]
        [Produces(typeof(List<Fornecedor>))]
        [Route("GetFornecedor")]
        public IActionResult GetFornecedor()
        {
            try
            {
                List<Fornecedor> lista = null;//_FornecedorService.GetFornecedor();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        
    }
}
