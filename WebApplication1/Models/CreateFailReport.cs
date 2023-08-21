using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApplication1.Models
{
    public class CreateFailReport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Report Enquiry ID")]
        public int FailureReportID { get; set; }

        [Required]
        [Display(Name = "Staff ID")]
        public int StaffID { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [MaxLength(255)]
        [Display(Name = "Response")]
        public string Response { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "0";
    }
}
