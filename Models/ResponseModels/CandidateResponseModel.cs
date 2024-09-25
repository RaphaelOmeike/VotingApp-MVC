namespace VotingApp.Models.ResponseModels
{
    public class CandidateResponseModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public StudentResponseModel? Student { get; set; }
        public List<CandidatePositionResponseModel> CandidatePositions { get; set; } = [];
    }
}
