using Dto.Purchasing.Request;
using Dto.Purchasing.Response;
using static Dto.Response.ServiceResponses;

namespace WebBlazor.Services.Contracts
{
    public interface IPurchasingService
    {
        Task<IEnumerable<GetPurchaseOrder>> GetPurchaseOrders();
        Task<GeneralResponse> CreatePo(CreatePO request);
    }
}
