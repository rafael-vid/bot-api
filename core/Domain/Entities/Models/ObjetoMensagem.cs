using Newtonsoft.Json;
using System.Collections.Generic;

namespace core.Domain.Entities.Models
{
    public class Metadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class Profile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Contact
    {
        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class Message
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("text")]
        public Dictionary<string, string> Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Change
    {
        [JsonProperty("value")]
        public Value Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class Value
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }

        [JsonProperty("statuses")]  // Add this property for unification
        public List<Status> Statuses { get; set; }
    }

    public class Status
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string StatusValue { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }

        [JsonProperty("conversation")]
        public Conversation Conversation { get; set; }

        [JsonProperty("pricing")]
        public Pricing Pricing { get; set; }
    }

    public class Conversation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("origin")]
        public Origin Origin { get; set; }
    }

    public class Origin
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Pricing
    {
        [JsonProperty("billable")]
        public bool Billable { get; set; }

        [JsonProperty("pricing_model")]
        public string PricingModel { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }

    public class Entry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<Change> Changes { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<Entry> Entry { get; set; }
    }
}
