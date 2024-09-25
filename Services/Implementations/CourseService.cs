using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;

namespace VotingApp.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public BaseResponse<CourseResponseModel> CreateCourse(CourseRequestModel request)
        {
            var courseExists = _courseRepository.Exists(c => c.Name == request.Name);
            if (courseExists)
            {
                return new BaseResponse<CourseResponseModel>
                {
                    Message = "Course with such name already exists! Registration failed!"
                };
            }
            Course course = new Course
            {
                Name = request.Name,
                Description = request.Description
            };
            _courseRepository.Create(course);
            _courseRepository.Save();
            return new BaseResponse<CourseResponseModel>
            {
                Data = new CourseResponseModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                },
                Status = true,
                Message = "Course created successfully!"
            };
        }

        public BaseResponse<ICollection<CourseResponseModel>> GetAllCourses()
        {
            var courses = _courseRepository.GetAll();
            if (!courses.Any())
            {
                return new BaseResponse<ICollection<CourseResponseModel>>
                {
                    Message = "No Active Courses",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<CourseResponseModel>>
            {
                Data = courses.Select(course => new CourseResponseModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    Students = course.Students.Select(c => new StudentResponseModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        MatricNo = c.MatricNo,
                        Email = c.Email,
                        CourseId = c.CourseId,
                        Gender = c.Gender,
                        Level = c.Level,
                        CGPA = c.CGPA,
                        CanVote = c.CanVote
                    }).ToList(),
                    Rules = course.Rules.Select(rule => new RuleResponseModel
                    {
                        Id = rule.Id,
                        Name = rule.Name,
                        Gender = rule.Gender,
                        CourseId = rule.CourseId,
                        MinCGPA = rule.MinCGPA,
                        MaxLevel = rule.MaxLevel,
                        MinLevel = rule.MinLevel,
                    }).ToList()
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public BaseResponse<CourseResponseModel> GetCourse(Guid id)
        {
            var course = _courseRepository.Get(c => c.Id == id);
            if (course == null)
            {
                return new BaseResponse<CourseResponseModel>
                {
                    Message = "Course with such id does not exist! Course not found!"
                };
            }
            return new BaseResponse<CourseResponseModel>
            {
                Data = new CourseResponseModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                },
                Status = true,
                Message = "Course found!"
            };
        }

        public BaseResponse<CourseResponseModel> GetCourseByName(string name)
        {
            var course = _courseRepository.Get(c => c.Name == name);
            if (course == null)
            {
                return new BaseResponse<CourseResponseModel>
                {
                    Message = "Course with such name does not exist! Course not found!"
                };
            }
            return new BaseResponse<CourseResponseModel>
            {
                Data = new CourseResponseModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                },
                Status = true,
                Message = "Course found!"
            };
        }

        public BaseResponse<CourseRequestModel> GetUpdateCourse(Guid id)
        {
            var course = _courseRepository.Get(c => c.Id == id);
            if (course == null)
            {
                return new BaseResponse<CourseRequestModel>
                {
                    Message = "Course with such id does not exist! Course not found!"
                };
            }
            return new BaseResponse<CourseRequestModel>
            {
                Data = new CourseRequestModel
                {
                    Name = course.Name,
                    Description = course.Description,
                },
                Status = true,
                Message = "Course found!"
            };
        }

        public BaseResponse<CourseResponseModel> UpdateCourse(Guid id, CourseRequestModel request)
        {
            var course = _courseRepository.Get(c => c.Id == id);
            if (course == null)
            {
                return new BaseResponse<CourseResponseModel>
                {
                    Message = "Course with such id does not exist! Course not found!"
                };
            }
            var nameExists = _courseRepository.Exists(c => c.Name == request.Name && c.Id != course.Id);
            if (nameExists)
            {
                return new BaseResponse<CourseResponseModel>
                {
                    Message = "Name of course already used! Course not found!"
                };
            }
            course.Name = request.Name;
            course.Description = request.Description;
            _courseRepository.Update(course);
            _courseRepository.Save();
            return new BaseResponse<CourseResponseModel>
            {
                Data = new CourseResponseModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                },
                Status = true,
                Message = "Course updated!"
            };
        }
    }
}
