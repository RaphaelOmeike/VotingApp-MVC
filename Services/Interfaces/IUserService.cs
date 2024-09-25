using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface IUserService
    {
        BaseResponse<UserResponseModel> LoginUser(UserRequestModel request);
        BaseResponse<UserResponseModel> GetUser(Guid id);
        User? GetCurrentUser();
        BaseResponse<UserResponseModel> GetUserByEmail(string email);
        BaseResponse<ICollection<UserResponseModel>> GetAllUsers();
        BaseResponse<UserResponseModel> UpdatePassword(Guid id, UpdateUserRequestModel request);
    }
}
