using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ShippingRateModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Shipping Rate ID")]
        public int ShippingRateID { get; set; }

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
        [Display(Name = "Shipping Rate")]
        public decimal ShippingRate { get; set; }

        [StringLength(3)]
        [Required]
        [Display(Name = "Currency")]
        public string Currency { get; set; } = "SGD";

        [Required]
        [Display(Name = "Transit Time")]
        public int TransitTime { get; set; } = 1;

        [Required]
        [Display(Name = "Last Updated By")]
        public int LastUpdatedBy { get; set; }
    }
}
