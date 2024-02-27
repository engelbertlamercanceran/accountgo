using System;
using System.ComponentModel.DataAnnotations;

namespace Dto.Purchasing.Request
{
    public class CreatePOLine : BaseDto
    {
        public CreatePOLine()
        {
            RowId = Guid.NewGuid();
        }
        public Guid RowId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int? ItemId { get; set; }

        public string ItemName { get; set; }
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? MeasurementId { get; set; }

        public string MeasurementName { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "The value must be >= 0")]
        [Required(ErrorMessage = "Required")]
        public decimal? Quantity { get; set; }

        [Required(ErrorMessage = "Required")]
        public decimal? Amount { get; set; }

        public decimal? Discount { get; set; }

        public decimal ComputeAmount()
        {
            decimal rate = Cost;
            decimal qty = Quantity ?? 0;
            decimal discount = Discount ?? 0;
            Amount = (rate * qty) - discount;

            return Amount ?? 0;
        }
    }
}
