using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class FeedbackEnquiry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Feedback Enquiry ID")]
        public int FeedbackEnquiryID { get; set; }

        [Required]
        [Display(Name = "Member ID")]
        public int MemberID { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Date and Time Posted")]
        public DateTime DateTimePosted { get; set; }

        public int? StaffID { get; set; }

        [MaxLength(255)]
        [Display(Name = "Response")]
        public string? Response { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "0";
    }
}
