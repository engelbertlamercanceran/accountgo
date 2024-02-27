using Dto.Enum;
using System;

namespace Dto.Purchasing.Response
{
    public class GetPurchaseOrder : BaseDto
    {
        public string No { get; set; }
        public int? PaymentTermId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal Amount { get { return GetTotalAmount(); } }
        public bool Completed { get; set; }
        public string ReferenceNo { get; set; }
        public int StatusId { get; set; }
        public string Status
        {
            get
            {
                return ((PurchaseOrderStatus)StatusId).ToString();
            }
        }
        public System.Collections.Generic.IList<GetPurchaseOrderLine> PurchaseOrderLines { get; set; }
        public GetPurchaseOrder()
        {
            PurchaseOrderLines = new System.Collections.Generic.List<GetPurchaseOrderLine>();
        }

        private decimal GetTotalAmount()
        {
            return GetTotalAmountLessTax();
        }

        private decimal GetTotalAmountLessTax()
        {
            decimal total = 0;
            foreach (var line in PurchaseOrderLines)
            {
                decimal quantityXamount = (line.Amount.Value * line.Quantity.Value);
                decimal discount = 0;
                if (line.Discount.HasValue)
                    discount = (line.Discount.Value / 100) > 0 ? (quantityXamount * (line.Discount.Value / 100)) : 0;
                total += ((line.Amount.Value * line.Quantity.Value) - discount);
            }
            return total;
        }
    }
}
