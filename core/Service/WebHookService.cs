using core.Domain.Entities;
using core.Infra.Repository;
using System;

namespace core.Service
{
    public class WebHookService : IWebHookService
    {
        private readonly WebHookRepository _repository;

        public WebHookService(WebHookRepository repository)
        {
            _repository = repository;
        }

        public void SaveMessage(WebHook objeto)
        {
            _repository.InsertMessage(objeto);
        }

        internal void SaveMessageSpam(WebHook responseData)
        {
            _repository.SaveMessageSpam(responseData);
        }

        internal void SaveMessageStatus(WebHook responseData)
        {
            _repository.InsertMessageStatus(responseData);
        }
    }
}
