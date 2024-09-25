namespace VotingApp.Models.ResponseModels
{
    public class VoteResponseModel
    {
        public Guid CandidatePositionId { get; set; }
        public CandidatePositionResponseModel? CandidatePosition { get; set; }
        public Guid StudentId { get; set; }
        public DateTime DateCasted { get; set; }
    }
}
