using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class ObjetoAction
    {
        [JsonProperty(PropertyName = "Action")]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "Categoria")]
        public string Categoria { get; set; }

    }
}

