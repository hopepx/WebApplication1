using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Staff
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Staff ID")]
        public int StaffID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Login ID")]
        public string LoginID { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "p@55Staff";

        [MaxLength(50)]
        [Display(Name = "Appointment")]
        public string Appointment { get; set; }

        [MaxLength(20)]
        [Display(Name = "Office Tel. No.")]
        public string OfficeTelNo { get; set; }

        [MaxLength(50)]
        [Display(Name = "Location")]
        public string Location { get; set; }

    }

}
