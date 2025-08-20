using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class LogDebug
    {
        [JsonProperty(PropertyName = "Telefone")]
        public string Telefone { get; set; }

        [JsonProperty(PropertyName = "Mensagem")]
        public string Mensagem { get; set; }
    }
}

