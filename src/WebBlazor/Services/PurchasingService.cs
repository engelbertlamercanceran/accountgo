using Dto.Authentication;
using Dto.Purchasing;
using Dto.Purchasing.Request;
using Dto.Purchasing.Response;
using Dto.Response;
using System.Net;
using WebBlazor.Services.Contracts;
using static Dto.Response.ServiceResponses;

namespace WebBlazor.Services
{
    public class PurchasingService(HttpClient httpClient) : IPurchasingService
    {
        private const string baseUrl = "/api/purchasing";
        
        public async Task<IEnumerable<GetPurchaseOrder>> GetPurchaseOrders()
        {
            //PurchaseOrders
            var httpResponse = await httpClient.GetAsync($"{baseUrl}/purchaseorders");

            if (!httpResponse.IsSuccessStatusCode)
            {
                return Enumerable.Empty<GetPurchaseOrder>();
            }

            return Generics.DeserializeJsonString<IEnumerable<GetPurchaseOrder>>(await httpResponse.Content.ReadAsStringAsync());
        }

        public async Task<GeneralResponse> CreatePo(CreatePO request)
        {
            var response = await httpClient
                 .PostAsync($"{baseUrl}/savepurchaseorder",
                 Generics.GenerateStringContent(
                 Generics.SerializeObj(request)));

            var apiResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Generics.DeserializeJsonString<GeneralResponse>(apiResponse);
                return error;
            }

            return Generics.DeserializeJsonString<GeneralResponse>(apiResponse);
        }
    }
}
