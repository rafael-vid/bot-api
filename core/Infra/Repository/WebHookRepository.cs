using core.Domain.Entities;
using core.Domain.Interfaces;
using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;

namespace core.Infra.Repository
{
    public class WebHookRepository
    {
        private readonly IRepositoryBase _RepositoryBase;


        public WebHookRepository(IRepositoryBase Repository)
        {
            _RepositoryBase = Repository;
        }
        public void InsertMessage(WebHook objeto)
        {
            using (var conn = _RepositoryBase.connMysql())
            {
                string text = "";
                string button = "";

                if (objeto.Entry[0].changes[0].value.messages[0].type == "button")
                {
                    button = objeto.Entry[0].changes[0].value.messages[0].button.text;
                }
                else
                {
                    text = objeto.Entry[0].changes[0].value.messages[0].text.body;
                }

                string sql = @"INSERT INTO WebHook (objeto, entry_id, change_field, messaging_product, display_phone_number,
                            phone_number_id, contact_name, wa_id, message_from, message_id, message_timestamp, message_body, message_type, resp_btn)
                            VALUES (@objeto, @entry_id, @change_field, @messaging_product, @display_phone_number, @phone_number_id, @contact_name, @wa_id, @message_from, @message_id, @message_timestamp, @message_body, @message_type, @resp_btn)";

                conn.Execute(sql, new
                {
                    @objeto = objeto.@object,
                    @entry_id = objeto.Entry[0].id,
                    @change_field = objeto.Entry[0].changes[0].field,
                    @messaging_product = objeto.Entry[0].changes[0].value.messaging_product,
                    @display_phone_number = objeto.Entry[0].changes[0].value.metadata.display_phone_number,
                    @phone_number_id = objeto.Entry[0].changes[0].value.metadata.phone_number_id,
                    @contact_name = objeto.Entry[0].changes[0].value.contacts[0].profile.name,
                    @wa_id = objeto.Entry[0].changes[0].value.contacts[0].wa_id,
                    @message_from = objeto.Entry[0].changes[0].value.messages[0].from,
                    @message_id = objeto.Entry[0].changes[0].value.messages[0].id,
                    @message_timestamp = objeto.Entry[0].changes[0].value.messages[0].timestamp,
                    @message_body = text,
                    @message_type = objeto.Entry[0].changes[0].value.messages[0].type,
                    @resp_btn = button

                });
            }
        }

        internal void InsertMessageStatus(WebHook responseData)
        {
            using (var conn = _RepositoryBase.connMysql())
            {
                string text = "";
                string button = "";

                if (responseData.Entry[0].changes[0].value.statuses.Count > 0)
                {


                    string sql = @"delete from webhook_status where Id= '" + responseData.Entry[0].changes[0].value.statuses[0].id + @"' and Status = 'failed'; 
                                   INSERT INTO webhook_status (Id, Status)
                                     VALUES (@Id, @Status)";

                    conn.Execute(sql, new
                    {
                        Id = responseData.Entry[0].changes[0].value.statuses[0].id,
                        Status = responseData.Entry[0].changes[0].value.statuses[0].status

                    });
                }
            }
        }

        internal void SaveMessageSpam(WebHook responseData)
        {
            using (var conn = _RepositoryBase.connMysql())
            {
                string text = "";
                string button = "";

                if (responseData.Entry[0].changes[0].value.statuses.Count > 0)
                {


                    string sql = @"INSERT INTO webhook_spam (Id)
                                     VALUES (@Id)";

                    conn.Execute(sql, new
                    {
                        Id = responseData.Entry[0].changes[0].value.statuses[0].id

                    });
                }
            }
        }
    }
}
