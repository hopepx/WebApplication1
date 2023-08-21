using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class CashVoucher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Cash Voucher ID")]
        public int CashVoucherID { get; set; }

        [Required]
        [DisplayName("Staff ID")]
        public int StaffID { get; set; }

        [Required]
        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3)]
        [DisplayName("Currency")]
        public string Currency { get; set; } = "SGD";

        [Required]
        [StringLength(1)]
        [DisplayName("Issuing Code")]
        public string IssuingCode { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Receiver Name")]
        public string ReceiverName { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression(@"^\+\d{10,15}$")]
        [DisplayName("Receiver Telephone Number")]
        public string ReceiverTelNo { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Date and Time Issued")]
        public DateTime DateTimeIssued { get; set; }

        [Required]
        [StringLength(1)]
        [DisplayName("Status")]
        public string Status { get; set; } = "0";
    }
}
