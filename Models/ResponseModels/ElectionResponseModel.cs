namespace VotingApp.Models.ResponseModels
{
    public class ElectionResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsClosed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid SessionId { get; set; }
        public SessionResponseModel? Session { get; set; }
        public Guid RuleId { get; set; }
        public List<PositionResponseModel> Positions { get; set; } = [];
    }
}
