using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class BuscarMensagem
    {
        public int id { get; set; }
        public int bloco { get; set; }
        public string mensagem { get; set; }
        public string proximoEstado { get; set; }
        public int? midia { get; set; }
        public string url { get; set; }
    }
}
