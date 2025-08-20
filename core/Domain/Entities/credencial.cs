using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class credencial 
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "User")]
        public string User { get; set; }

        [JsonProperty(PropertyName = "Secret")]
        public string Secret { get; set; }

        [JsonProperty(PropertyName = "Origem")]
        public string Origem { get; set; }

        [JsonProperty(PropertyName = "Expiration")]
        public int Expiration { get; set; }

    }
}

