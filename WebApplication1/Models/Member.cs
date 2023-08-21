using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace WebApplication1.Models
{
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Member ID")]
        public int MemberID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(5)]
        [Display(Name = "Salutation")]
        public string Salutation { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression(@"^\+\d{1,3}\d{7,15}$", ErrorMessage = "Phone number should start with a '+' and contain only numbers.")]
        [Display(Name = "Contact Number")]
        public string TelNo { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email Address")]
        public string EmailAddr { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}
