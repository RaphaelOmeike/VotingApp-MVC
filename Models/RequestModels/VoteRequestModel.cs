namespace VotingApp.Models.RequestModels
{
    public class VoteRequestModel
    {
        public Guid CandidateId { get; set; }
        public Guid PositionId { get; set; }
        public Guid StudentId { get; set; }
    }
}
