using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class ObjetoLogar
    {
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "Senha")]
        public string Senha { get; set; }

        [JsonProperty(PropertyName = "Origem")]
        public string Origem { get; set; }

    }
}

