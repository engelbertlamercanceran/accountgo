namespace Dto.Purchasing.Response
{
    public class GetPurchaseOrderLine : BaseDto
    {
        public int? ItemId { get; set; }
        public int? MeasurementId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? RemainingQtyToInvoice { get; set; }
    }
}
