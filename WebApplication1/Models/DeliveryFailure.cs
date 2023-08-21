using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class DeliveryFailure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Report ID")]
        public int ReportID { get; set; }

        [DisplayName("Parcel ID")]
        public int ParcelID { get; set; }

        [DisplayName("Delivery Man ID")]
        public int DeliveryManID { get; set; }

        [Required]
        [StringLength(1)]
        [RegularExpression("^[1-4]$", ErrorMessage = "Invalid failure type.")]
        [DisplayName("Failure Type")]
        public string FailureType { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [DisplayName("Station Manager ID")]
        public int? StationMgrID { get; set; }

        [MaxLength(255)]
        [DisplayName("Follow Up Action")]
        public string FollowUpAction { get; set; }

        [Required]
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }
    }
}
