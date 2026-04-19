using System.ComponentModel.DataAnnotations;

namespace PUSL2020_Blind_Match_PAS.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Research Area Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Area names should only contain letters.")]
        public string Name { get; set; }
    }
}
