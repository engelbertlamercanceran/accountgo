using Dto.Purchasing.Response;
using Radzen.Blazor;
using WebBlazor.Enums;

namespace WebBlazor.Pages.Vendor.PurchaseOrder
{
    public class PurchaseOrderBase : SuperComponentBase
    {

        protected RadzenDataGrid<GetPurchaseOrder> grid { get; set; } = default!;
        protected IEnumerable<GetPurchaseOrder> items = Enumerable.Empty<GetPurchaseOrder>();
        protected bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            var response = await purchasingService.GetPurchaseOrders();
            items = response;
            isLoading = false;
        }

        protected void GotoCreatePo()
        {
            navManager.NavigateTo(PageRoutes.Vendor.CreatePO);
        }
    }


}
