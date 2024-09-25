using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface ICourseService
    {
        BaseResponse<CourseResponseModel> CreateCourse(CourseRequestModel request);
        BaseResponse<CourseResponseModel> GetCourse(Guid id);
        BaseResponse<ICollection<CourseResponseModel>> GetAllCourses();
        BaseResponse<CourseResponseModel> GetCourseByName(string name);
        BaseResponse<CourseRequestModel> GetUpdateCourse(Guid id);
        BaseResponse<CourseResponseModel> UpdateCourse(Guid id, CourseRequestModel request);
    }
}
