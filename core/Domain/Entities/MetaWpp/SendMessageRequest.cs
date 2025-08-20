using System;
using System.Collections.Generic;

namespace core.Domain.Entities.MetaWpp
{
    public class SendMessageRequest
    {
        public string name { get; set; }
        public string to { get; set; }
        public string? idMeta { get; set; }
        public string[]? parameters { get; set; }

        public string? image { get; set; }
    }
}
