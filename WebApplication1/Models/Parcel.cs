using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Parcel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Parcel ID")]
        public int ParcelID { get; set; }

        [MaxLength(255, ErrorMessage = "Item Description cannot exceed 255 characters")]
        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Sender Name cannot exceed 50 characters")]
        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Sender Phone Number cannot exceed 20 characters")]
        [RegularExpression(@"^\+\d{1,3}\d{7,15}$", ErrorMessage = "Phone number should start with a '+' and contain only numbers.")]
        [Display(Name = "Sender Phone Number")]
        public string SenderTelNo { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Receiver Name cannot exceed 50 characters")]
        [Display(Name = "Receiver Name")]
        public string ReceiverName { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Receiver Phone Number cannot exceed 20 characters")]
        [RegularExpression(@"^\+\d{1,3}\d{7,15}$", ErrorMessage = "Phone number should start with a '+' and contain only numbers.")]
        [Display(Name = "Receiver Phone Number")]
        public string ReceiverTelNo { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Delivery Address cannot exceed 255 characters")]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "From City")]
        public string FromCity { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "From Country")]
        public string FromCountry { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "To City")]
        public string ToCity { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "To Country")]
        public string ToCountry { get; set; }

        [Required]
        [Display(Name = "Parcel Weight")]
        public double ParcelWeight { get; set; } = 0;

        [Required]
        [Display(Name = "Delivery Charge")]
        public decimal DeliveryCharge { get; set; } = 0m;

        [Required]
        [MaxLength(3)]
        [Display(Name = "Currency")]
        public string Currency { get; set; } = "SGD";

        [Display(Name = "Target Delivery Date")]
        public DateTime? TargetDeliveryDate { get; set; }

        [Required]
        [Display(Name = "Delivery Status")]
        public string DeliveryStatus { get; set; } = "0";

        [Display(Name = "Delivery Man ID")]
        public int? DeliveryManID { get; set; }

    }
}
