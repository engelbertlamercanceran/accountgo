using Dto.Inventory.Response;
using Dto.Purchasing.Request;
using Dto.Purchasing.Response;
using Dto.TaxSystem;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Threading.Tasks;
using WebBlazor.Enums;

namespace WebBlazor.Pages.Vendor.PurchaseOrder.CreatePurchaseOrder
{
    public class CreatePurchaseOrderBase : SuperComponentBase
    {
        protected RadzenDataGrid<CreatePOLine> poLineGrid { get; set; } = default!;
        protected List<CreatePOLine> poLineToInsert { get; set; } = new();
        protected List<CreatePOLine> poLineToUpdate { get; set; } = new();
        protected CreatePO formModel { get; set; } = new();

        protected async Task OnValidSubmit()
        {
            DispatchLoadingEvent(true);

            try
            {
                var unSavedItems = poLineToInsert.Union(poLineToUpdate).ToList();

                var validUnsavedItems = unSavedItems.Where(x => x.ItemId != null);

                if (validUnsavedItems.Any())
                {
                    foreach (var item in validUnsavedItems)
                    {
                        await poLineGrid.UpdateRow(item);
                    }
                }

                var res = await purchasingService.CreatePo(formModel);
                navManager.NavigateTo(PageRoutes.Vendor.PurchaseOrders);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                DispatchLoadingEvent(false);
            }
        }

        #region PO Line after bind logic
        protected async Task OnAfterItemBind(CreatePOLine itemLine)
        {
            var items = await lookupService.GetItems();
            var item = items.Single(x => x.Id == itemLine.ItemId);

            itemLine.ItemName = item.Name;
            itemLine.Quantity = 1;
            itemLine.Cost = item.Cost;
            itemLine.Discount = 0;
            itemLine.ComputeAmount();

            itemLine.MeasurementId = item.Measurement.Id;
            await OnAfterMeasurementBind(itemLine);
        }

        protected  void OnComputeAmount(CreatePOLine itemLine)
        {
            itemLine.ComputeAmount();
        }

        protected async Task OnAfterMeasurementBind(CreatePOLine itemLineUom)
        {
            var measurements = await lookupService.GetMeasurements();
            var measurement = measurements.Single(x => x.Id == itemLineUom.MeasurementId);

            itemLineUom.MeasurementName = measurement.Description;
        }

        #endregion

        #region Inline edit grid controls
        protected async Task InsertRow()
        {
            var poLine = new CreatePOLine();
            poLineToInsert.Add(poLine);
            await poLineGrid.InsertRow(poLine);
        }

        protected async Task EditRow(CreatePOLine lineItem)
        {
            poLineToUpdate.Add(lineItem);
            await poLineGrid.EditRow(lineItem);

        }

        protected async Task SaveRow(CreatePOLine lineItem)
        {
            await poLineGrid.UpdateRow(lineItem);
        }

        protected void CancelEdit(CreatePOLine lineItem)
        {
            ResetInsert(lineItem);
            ResetEdit(lineItem);

            poLineGrid.CancelEditRow(lineItem);
        }

        protected async Task DeleteRow(CreatePOLine lineItem)
        {
            ResetInsert(lineItem);

            ResetEdit(lineItem);

            var savedItem = formModel.PurchaseOrderLines.FirstOrDefault(x=> x.RowId == lineItem.RowId);

            if (savedItem != null)
            {
                formModel.PurchaseOrderLines.Remove(savedItem);
            }

            await poLineGrid.Reload();

            //for some weird behaviour, radzen clears the grid items that are not in saved state when calling Reload
            //solution for now is to reinsert
            foreach(var item in poLineToInsert)
            {
                await poLineGrid.InsertRow(item);
            }
        }

        protected bool ResetInsert(CreatePOLine lineItem)
        {
            var itemToInsert = poLineToInsert.FirstOrDefault(x => x.RowId == lineItem.RowId);

            if (itemToInsert != null)
            {
                poLineToInsert.Remove(itemToInsert);
                return true;
            }
            return false;
        }

        protected bool ResetEdit(CreatePOLine lineItem)
        {
            var itemToUpdate = poLineToUpdate.FirstOrDefault(x => x.RowId == lineItem.RowId);
            if (itemToUpdate != null)
            {
                poLineToUpdate.Remove(itemToUpdate);
                return true;
            }

            return false;
        }

        protected void OnCreateRow(CreatePOLine lineItem)
        {
            //TODO: validate before doing anything

            ResetInsert(lineItem);

            formModel.PurchaseOrderLines.Add(lineItem); 
        }

        protected void OnUpdateRow(CreatePOLine lineItem)
        {
            //TODO: validate before doing anything

            ResetEdit(lineItem);

            var item = formModel.PurchaseOrderLines.FirstOrDefault(x=> x.RowId == lineItem.RowId);

            if(item != null)
            {
                item = lineItem;
            }
            else
            {
                formModel.PurchaseOrderLines.Add(lineItem);
            }
        }
        #endregion
    }
}
