using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface ICandidateService
    {
        BaseResponse<CandidateResponseModel> CreateCandidate(CandidateRequestModel request);
        BaseResponse<CandidateResponseModel> GetCandidate(Guid id);
        BaseResponse<CandidateResponseModel> GetCandidateByStudentId(Guid id);
        BaseResponse<ICollection<CandidateResponseModel>> GetAllCandidates();
        BaseResponse<CandidateResponseModel> GetCandidateByEmail(string email);
    }
}
