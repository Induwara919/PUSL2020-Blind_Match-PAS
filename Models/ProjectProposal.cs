namespace PUSL2020_Blind_Match_PAS.Models
{
    public class ProjectProposal
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public string TechStack { get; set; } = string.Empty;
        public string ResearchArea { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public bool IsRevealed { get; set; } = false;

        public string StudentId { get; set; } = string.Empty;
        public string? SupervisorId { get; set; }
    }
}