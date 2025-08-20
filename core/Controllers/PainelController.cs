using core.Domain.Entities;
using core.Models;
using core.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace core.Controllers
{
    public class PainelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DadosFornecedor()
        {
            return View();
        }
    }
}
