using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface IVoteCastingInfoService
    {
        BaseResponse<VoteResponseModel> CreateVoteCastingInfo(Guid studentId, Guid contestantId);
        BaseResponse<VoteResponseModel> VoteCastingInfoExists(Guid studentId, Guid positionId);
        BaseResponse<ICollection<CandidatePositionResponseModel>> GetLiveResults(Guid electionId);
        BaseResponse<CandidatePositionResponseModel> GetWinner(Guid electionId, Guid positionId);
        public BaseResponse<ICollection<VoteResponseModel>> GetAllVotesByStudent(Guid studentId);
        BaseResponse<CandidatePositionResponseModel> GetAllVotesForContestant(Guid candidateId, Guid positionId);
        BaseResponse<VoteResponseModel> GetVoteCastingInfo(Guid studentId, Guid candidateId, Guid positionId);
        BaseResponse<VoteResponseModel> GetVoteCastingInfoById(Guid studentId, Guid candidatePositionId);
        BaseResponse<ICollection<VoteResponseModel>> GetAll();
    }
}
