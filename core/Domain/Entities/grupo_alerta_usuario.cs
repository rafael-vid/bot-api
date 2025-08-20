using System;
using Newtonsoft.Json;

namespace core.Domain.Entities
{
    public class grupo_alerta_usuario
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id{ get; set; }

        [JsonProperty(PropertyName = "Nome")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "Telefone")]
        public string Telefone { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
    }
}
