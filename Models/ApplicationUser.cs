using Microsoft.AspNetCore.Identity;

namespace PUSL2020_Blind_Match_PAS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? StudentId { get; set; } 
    }
}