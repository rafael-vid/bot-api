using core.Domain.Entities.MetaWpp;
using core.Models;
using core.Util;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace core.Service
{
    public class MetaWppService : IMetaWppService
    {
        public async Task<ResponseModel<string>> SendMessageMetaAsync(SendMessageRequest request)
        {
            HttpClient client = new HttpClient();

            Settings app = new Settings();
            string IdentificadorTelefone = string.Empty;
            if (request.idMeta.HasValue())
            {
                IdentificadorTelefone = request.idMeta;
            }
            else
            {
                IdentificadorTelefone = app.Appsettings("IdentificadorTelefone");
            }
            
            string Access_token_Meta = app.Appsettings("Access_token_Meta");

            string apiUrl = $"https://graph.facebook.com/v18.0/{IdentificadorTelefone}/messages";
            var _request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Access_token_Meta);

            var parameters = new List<Parameters>();
            var component = new List<Component>();

            var parametersImg = new List<Parameters>();

            if (request.parameters != null && request.parameters.Length > 0)
            {
                if (request.image != null)
                {
                    parametersImg.Add(new Parameters { Type = "image", image = new Image { link = request.image } });

                    component.Add(new Component { Type = "header", parameters = parametersImg });
                }

                foreach (var item in request.parameters)
                {
                    parameters.Add(new Parameters { text = item , Type = "text" });
                }

                component.Add(new Component { Type = "body", parameters = parameters });
            }

            var template = new Template
            {
                name = request.name,
                language = new Language
                {
                    code = "pt_BR"
                },
                components = component.Count > 0 ? component : null
            };

            var data = new MessageRequest { 
                messaging_product = "whatsapp",
                to = request.to,
                Type = "template",
                template = template
            };

            var req = System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull}).Replace("Type", "type");

            _request.Content = new StringContent(req, System.Text.Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await client.SendAsync(_request);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                MessageResponse json = JsonConvert.DeserializeObject<MessageResponse>(responseBody);

                return new ResponseModel<string>
                (
                    success: true,
                    statusCode: 200,
                    result: json.messages[0].id
                );
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(responseBody);

                var err = json.error.message;

                return new ResponseModel<string>
                (
                    success: false,
                    statusCode: ((int)response.StatusCode),
                    result: null,
                     new Dictionary<string, List<string>>()
                     {
                         {"Message", new List<string>(){ (string)err } }
                     }
                );
            }
        }
    }
}
