using System.Collections.Generic;

namespace core.Models
{
    public class MessageResponse
    {
        public string messaging_product { get; set; }
        public List<Contacts> contacts { get; set; }
        public List<Messages> messages { get; set; }
    }

    public class Messages
    {
        public string id { get; set; }
        public string message_status { get; set; }
    }

    public class Contacts
    {
        public string input { get; set; }
        public string wa_id { get; set; }
    }
}
