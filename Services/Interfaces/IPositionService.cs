using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface IPositionService
    {
        BaseResponse<PositionResponseModel> CreatePosition(PositionRequestModel request);
        BaseResponse<PositionResponseModel> StudentIsEligible(Guid studentId, Guid positionId);
        BaseResponse<PositionResponseModel> GetPosition(Guid id);
        BaseResponse<PositionResponseModel> ChangePositionStatus(Guid id);
        BaseResponse<ICollection<PositionResponseModel>> GetAllPositions();
        BaseResponse<ICollection<PositionResponseModel>> GetAllPositionsForElection(Guid electionId);
        BaseResponse<PositionResponseModel> GetPositionByElectionName(string positionName, Guid electionId);
        BaseResponse<PositionRequestModel> GetUpdatePosition(Guid id);
        BaseResponse<PositionResponseModel> UpdatePosition(Guid id, PositionRequestModel request);
    }
}
