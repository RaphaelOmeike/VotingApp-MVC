using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface ICandidatePositionService
    {
        BaseResponse<CandidatePositionResponseModel> CreateCandidatePosition(CandidatePositionRequestModel request);
        BaseResponse<CandidatePositionResponseModel> GetCandidatePosition(Guid candidateId, Guid positionId);
        BaseResponse<CandidatePositionResponseModel> ChangeContestantStatus(Guid candidatePositionId, Guid userId);
        BaseResponse<CandidatePositionResponseModel> GetCandidatePositionById(Guid id);
        BaseResponse<ICollection<CandidatePositionResponseModel>> GetAllCandidatePositions();
        BaseResponse<ICollection<CandidatePositionResponseModel>> GetAllContestantsForPosition(Guid positionId);
        BaseResponse<CandidatePositionRequestModel> GetUpdateCandidatePosition(Guid candidatePositionId);
        BaseResponse<CandidatePositionResponseModel> UpdateCandidatePosition(CandidatePositionRequestModel request);
        BaseResponse<CandidatePositionResponseModel> UpdateCandidateVotesNo(Guid id, int votesNo);
    }
}
