using core.Domain.Entities;

namespace core.Service
{
    public interface IWebHookService
    {
        void SaveMessage(WebHook objeto);
    }
}
