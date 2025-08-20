using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class Fornecedor
    {
        public string Nome { get; set; }
        public string Numero_Telefone { get; set; }
        public string estado { get; set; }
    }
}

