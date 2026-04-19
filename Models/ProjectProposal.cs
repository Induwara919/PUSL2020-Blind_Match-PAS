using System.ComponentModel.DataAnnotations;

namespace PUSL2020_Blind_Match_PAS.Models
{
    public class ProjectProposal
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A project title is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
        [Display(Name = "Project Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide an abstract.")]
        [StringLength(2000, ErrorMessage = "Abstract cannot exceed 2000 characters.")]
        [DataType(DataType.MultilineText)]
        public string Abstract { get; set; } = string.Empty;

        [Required(ErrorMessage = "Technical stack must be specified.")]
        [Display(Name = "Technical Stack")]
        public string TechnicalStack { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a research area.")]
        [Display(Name = "Research Area")]
        public string ResearchArea { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public bool IsIdentityRevealed { get; set; } = false;

        public string StudentId { get; set; } = string.Empty;
        public string? StudentName { get; set; }

        public string? SupervisorId { get; set; }
        public string? SupervisorName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? SupervisorContact { get; set; }
    }
}
