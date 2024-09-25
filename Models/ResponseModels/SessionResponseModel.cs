namespace VotingApp.Models.ResponseModels
{
    public class SessionResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<ElectionResponseModel> Elections { get; set; } = [];
    }
}
