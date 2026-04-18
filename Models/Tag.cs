using System.ComponentModel.DataAnnotations;

namespace PUSL2020_Blind_Match_PAS.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Research Area")]
        public string Name { get; set; } 
    }
}
