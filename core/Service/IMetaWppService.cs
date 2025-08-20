using core.Domain.Entities.MetaWpp;
using core.Models;
using System.Threading.Tasks;

namespace core.Service
{
    public interface IMetaWppService
    {
        Task<ResponseModel<string>> SendMessageMetaAsync(SendMessageRequest request);
    }
}
