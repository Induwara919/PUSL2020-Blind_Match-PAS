using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PUSL2020_Blind_Match_PAS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [PersonalData]
        [Display(Name = "Student ID")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Student ID must be exactly 8 digits.")]
        public string? StudentId { get; set; }
    }
}
