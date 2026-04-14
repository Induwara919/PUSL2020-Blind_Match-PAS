using System.ComponentModel.DataAnnotations;

namespace PUSL2020_Blind_Match_PAS.Models
{
    public class ProjectProposal
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Abstract is required")]
        [StringLength(2000)]
        public string Abstract { get; set; } = string.Empty;

        [Required(ErrorMessage = "Technical Stack is required")]
        public string TechnicalStack { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a Research Area")]
        public string ResearchArea { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";
        public bool IsIdentityRevealed { get; set; } = false;

        public string StudentId { get; set; } = string.Empty;
        public string? SupervisorId { get; set; }

        public string? StudentName { get; set; }
        public string? SupervisorName { get; set; }
        public string? SupervisorContact { get; set; }
    }
}