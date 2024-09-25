using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface ISessionService
    {
        BaseResponse<SessionResponseModel> CreateSession(SessionRequestModel request);
        BaseResponse<SessionResponseModel> GetSession(Guid id);
        BaseResponse<ICollection<SessionResponseModel>> GetAllSessions();
        BaseResponse<SessionResponseModel> GetSessionByName(string name);
        BaseResponse<SessionRequestModel> GetUpdateSession(Guid id);
        BaseResponse<SessionResponseModel> UpdateSession(Guid id, SessionRequestModel request);
    }
}
