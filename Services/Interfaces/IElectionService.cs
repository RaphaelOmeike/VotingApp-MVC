using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface IElectionService
    {
        BaseResponse<ElectionResponseModel> CreateElection(ElectionRequestModel request);
        BaseResponse<ElectionResponseModel> StudentIsEligible(Guid studentId, Guid positionId);
        BaseResponse<ElectionResponseModel> GetElection(Guid id);
        BaseResponse<ICollection<ElectionResponseModel>> GetAllElections();
        BaseResponse<ElectionResponseModel> GetElectionBySessionName(string electionName, string sessionName);
        BaseResponse<ElectionRequestModel> GetUpdateElection(Guid id);
        BaseResponse<ElectionResponseModel> UpdateElection(Guid id, ElectionRequestModel request);
        BaseResponse<ElectionResponseModel> EndElection();
        
    }
}
