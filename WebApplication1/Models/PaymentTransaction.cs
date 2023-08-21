using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class PaymentTransaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Transaction ID")]
        public int TransactionID { get; set; }

        [Required]
        [Display(Name = "Parcel ID")]
        public int ParcelID { get; set; }

        [Display(Name = "Amount Transferred")]
        public decimal AmtTran { get; set; }

        [StringLength(3)]
        [Required]
        [Display(Name = "Currency")]
        public string Currency { get; set; } = "SGD";

        [StringLength(4)]
        [Required]
        [Display(Name = "Transaction Type")]
        public string TranType { get; set; } = "CASH";

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Transaction Date")]
        public DateTime TranDate { get; set; }
    }
}
