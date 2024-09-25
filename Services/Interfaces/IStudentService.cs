using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface IStudentService
    {
        BaseResponse<StudentResponseModel> CreateStudent(StudentRequestModel request);
        BaseResponse<ICollection<StudentResponseModel>> ReadStudentsFromFile(IFormFile studentFile);//accept file in controller also
        BaseResponse<StudentResponseModel> GetStudent(Guid id);
        BaseResponse<StudentResponseModel> ChangeVoteStatus(Guid id);
        BaseResponse<ICollection<StudentResponseModel>> GetAllStudents();
        BaseResponse<StudentResponseModel> GetStudentByEmail(string email);
        BaseResponse<UpdateStudentRequestModel> GetUpdateStudent(Guid id);
        BaseResponse<StudentResponseModel> UpdateStudent(Guid id, UpdateStudentRequestModel request);
    }
}
