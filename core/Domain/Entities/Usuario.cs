using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class Usuario
    {
        public string User { get; set; }
        public string Secret { get; set; }
    }
}

