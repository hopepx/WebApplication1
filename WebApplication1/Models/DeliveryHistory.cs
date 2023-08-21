using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class DeliveryHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Record ID")]
        public int RecordID { get; set; }

        [DisplayName("Parcel ID")]
        public int ParcelID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
