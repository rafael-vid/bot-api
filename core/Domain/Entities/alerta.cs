using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class alerta : alertaInsert
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "dataenvio")]
        public DateTime? dataenvio { get; set; }
        [JsonProperty(PropertyName = "dataconfirmacao")]
        public DateTime? dataconfirmacao { get; set; }

    }
}

