namespace VotingApp.Models.ResponseModels
{
    public class PositionResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ElectionId { get; set; }
        public ElectionResponseModel? Election { get; set; }
        public Guid RuleId { get; set; }
        public RuleResponseModel? Rule { get; set; }
        public List<CandidatePositionResponseModel> Contestants { get; set; } = [];
    }
}
