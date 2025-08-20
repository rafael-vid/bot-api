using Newtonsoft.Json;
using System.Collections.Generic;

namespace core.Models
{
    public class MessageRequest
    {
        public string messaging_product { get; set; }
        public string to { get; set; }
        public string Type { get; set; }
        public Template template { get; set; }
    }

    public class Template
    {
        public string name { get; set; }
        public Language language { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Component>? components { get; set; }
}

    public class Language
    {
        public string code { get; set; }
    }

    public class Component
    {
        public string Type { get; set; }
        public List<Parameters> parameters { get; set; }
    }

    public class Parameters
    {
        public string Type { get; set; }
        public string text { get; set; }
        public Image? image { get; set; }
    }

    public class Image
    {
        public string link { get; set; }

    }
}
