namespace VotingApp.Models.RequestModels
{
    public class ElectionRequestModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile Image { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid SessionId { get; set; }
        public Guid RuleId { get; set; }
    }
}
