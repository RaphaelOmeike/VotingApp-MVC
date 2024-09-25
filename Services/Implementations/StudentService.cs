using VotingApp.Models.Constants;
using VotingApp.Models.Entities;
using VotingApp.Models.Enums;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;

namespace VotingApp.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRoleRepository _roleRepository;
        public StudentService(IStudentRepository studentRepository, IUserRepository userRepository, IWebHostEnvironment webHostEnvironment, ICourseRepository courseRepository, IRoleRepository roleRepository)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _courseRepository = courseRepository;
            _roleRepository = roleRepository;
        }

        public BaseResponse<StudentResponseModel> ChangeVoteStatus(Guid id)
        {
            var student = _studentRepository.Get(c => c.Id == id);
            if (student == null)
            {
                return new BaseResponse<StudentResponseModel>
                {
                    Message = "Student with such id does not exist! Student not found!"
                };
            }
            student.CanVote = !student.CanVote;
            _studentRepository.Update(student);
            _studentRepository.Save();
            return new BaseResponse<StudentResponseModel>
            {
                Data = new StudentResponseModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    MatricNo = student.MatricNo,
                    Email = student.Email,
                    CourseId = student.CourseId,
                    Gender = student.Gender,
                    Level = student.Level,
                    CGPA = student.CGPA,
                    CanVote = student.CanVote
                },
                Status = true,
                Message = "Student voting status changed!"
            };
        }

        public BaseResponse<StudentResponseModel> CreateStudent(StudentRequestModel request)
        {
            var studentExists = _studentRepository.Exists(c => c.Email == request.Email);
            if (studentExists)
            {
                return new BaseResponse<StudentResponseModel>
                {
                    Message = "Student with such email already exists! Registration failed!"
                };
            }
            var matNo = _studentRepository.Exists(c => c.MatricNo == request.MatricNo);
            if (matNo)
            {
                return new BaseResponse<StudentResponseModel>
                {
                    Message = "Student with such matric no already exists! Registration failed!"
                };
            }
            var role = _roleRepository.Get(c => c.Name == RoleConst.Student);
            if (role == null)
            {
                return new BaseResponse<StudentResponseModel>
                {
                    Message = "Role does not exist. Registration failed!"
                };
            }
            Student student = new Student
            {
                Name = request.Name,
                MatricNo = request.MatricNo,
                Email = request.Email,
                CourseId = request.CourseId,
                Gender = request.Gender,
                Level = request.Level,
                CGPA = request.CGPA,
                CanVote = true
            };
            User user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = role.Id
            };//make some corrections here also
            _userRepository.Create(user);
            _studentRepository.Create(student);
            _studentRepository.Save();
            return new BaseResponse<StudentResponseModel>
            {
                Data = new StudentResponseModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    MatricNo = student.MatricNo,
                    Email = student.Email,
                    CourseId = student.CourseId,
                    Gender = student.Gender,
                    Level = student.Level,
                    CGPA = student.CGPA,
                    CanVote = student.CanVote
                },
                Status = true,
                Message = "Student created successfully!"
            };
        }

        public BaseResponse<ICollection<StudentResponseModel>> GetAllStudents()
        {
            var students = _studentRepository.GetAll();
            if (!students.Any())
            {
                return new BaseResponse<ICollection<StudentResponseModel>>
                {
                    Message = "No Active Students",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<StudentResponseModel>>
            {
                Data = students.Select(c => new StudentResponseModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    MatricNo = c.MatricNo,
                    Email = c.Email,
                    CourseId = c.CourseId,
                    Course = new CourseResponseModel
                    {
                        Id = c.Course.Id,
                        Name = c.Course.Name,
                        Description = c.Course.Description,
                    },
                    Gender = c.Gender,
                    Level = c.Level,
                    CGPA = c.CGPA,
                    CanVote = c.CanVote
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public BaseResponse<StudentResponseModel> GetStudent(Guid id)
        {
            var student = _studentRepository.Get(c => c.Id == id);
            if (student == null)
            {
                return new BaseResponse<StudentResponseModel>
                {
                    Message = "Student with such id does not exist! Student not found!"
                };
            }
            return new BaseResponse<StudentResponseModel>
            {
                Data = new StudentResponseModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    MatricNo = student.MatricNo,
                    Email = student.Email,
                    CourseId = student.CourseId,
                    Gender = student.Gender,
                    Level = student.Level,
                    CGPA = student.CGPA,
                    CanVote = student.CanVote
                },
                Status = true,
                Message = "Student found!"
            };
        }

        public BaseResponse<StudentResponseModel> GetStudentByEmail(string email)
        {
            var student = _studentRepository.Get(c => c.Email == email);
            if (student == null)
            {
                return new BaseResponse<StudentResponseModel>
                {
                    Message = "Student with such id does not exist! Student not found!"
                };
            }
            return new BaseResponse<StudentResponseModel>
            {
                Data = new StudentResponseModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    MatricNo = student.MatricNo,
                    Email = student.Email,
                    CourseId = student.CourseId,
                    Gender = student.Gender,
                    Level = student.Level,
                    CGPA = student.CGPA,
                    CanVote = student.CanVote
                },
                Status = true,
                Message = "Student found!"
            };
        }

        public BaseResponse<UpdateStudentRequestModel> GetUpdateStudent(Guid id)
        {
            var student = _studentRepository.Get(c => c.Id == id);
            if (student == null)
            {
                return new BaseResponse<UpdateStudentRequestModel>
                {
                    Message = "Student with such id does not exist! Student not found!"
                };
            }
            return new BaseResponse<UpdateStudentRequestModel>
            {
                Data = new UpdateStudentRequestModel
                {
                    Name = student.Name,
                    CourseId = student.CourseId,
                    Gender = student.Gender,
                    Level = student.Level,
                    CGPA = student.CGPA
                },
                Status = true,
                Message = "Student found!"
            };
        }

        public BaseResponse<ICollection<StudentResponseModel>> ReadStudentsFromFile(IFormFile studentFile)
        {
            if (studentFile == null)
            {
                return new BaseResponse<ICollection<StudentResponseModel>>
                {
                    Message = "Invalid file upload!"
                };
            }
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Stu_Files");
            string fileName = studentFile.FileName;
            string filePath = Path.Combine(uploadDir, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                studentFile.CopyTo(fileStream);
            }
            using (StreamReader reader = new StreamReader(filePath))
            {
                ICollection<StudentResponseModel> students = [];
                while (!reader.EndOfStream)
                {
                    var asd = reader.ReadLine();
                    if (asd == null)
                        continue;
                    string[] str = asd.Split(',');
                    try
                    {
                        StudentRequestModel student = new StudentRequestModel
                        {
                            Name = str[0],
                            Email = str[1].ToLower(),///
                            MatricNo = str[2].ToLower(),
                            Password = str[2],
                            Gender = (Gender)int.Parse(str[3]),
                            Level = (Levels)int.Parse(str[4]),
                            CGPA = decimal.Parse(str[5])
                        };
                        var course = _courseRepository.Get(c => c.Name == str[3].ToLower());
                        if (course == null)
                        {
                            student.CourseId = GenerateRandCourse();
                        }
                        else
                        {
                            student.CourseId = course.Id;
                        }
                        var response = CreateStudent(student);
                        if (response.Status && response.Data != null)
                        {
                            students.Add(response.Data);
                        }
                    }
                    catch
                    {
                        return new BaseResponse<ICollection<StudentResponseModel>>
                        {
                            Message = "Invalid file content format!",
                            Data = []
                        };
                    }
                }
                return new BaseResponse<ICollection<StudentResponseModel>>
                {
                    Data = students,
                    Status = true,
                    Message = "Students read from file successfully!"
                };
            }
        }

        public BaseResponse<StudentResponseModel> UpdateStudent(Guid id, UpdateStudentRequestModel request)
        {
            var student = _studentRepository.Get(c => c.Id == id);
            if (student == null)
            {
                return new BaseResponse<StudentResponseModel>
                {
                    Message = "Update failed! Student with such id does not exist!"
                };
            }
            student.Name = request.Name;
            student.CourseId = request.CourseId;
            student.Gender = request.Gender;
            student.Level = request.Level;
            student.CGPA = request.CGPA;

            _studentRepository.Update(student);
            _studentRepository.Save();
            return new BaseResponse<StudentResponseModel>
            {
                Data = new StudentResponseModel
                { 
                    Id = student.Id,
                    Name = student.Name,
                    MatricNo = student.MatricNo,
                    Email = student.Email,
                    CourseId = student.CourseId,
                    Gender = student.Gender,
                    Level = student.Level,
                    CGPA = student.CGPA,
                    CanVote = student.CanVote
                },
                Status = true,
                Message = "Student updated!"
            };
        }
        private Guid GenerateRandCourse()
        {
            var courses = _courseRepository.GetAll().ToList();
            Random generator = new Random();
            int num = generator.Next(0, courses.Count);
            var course = courses[num];//change 0 to 1 later to skip all courses correction
            while (course.Name == "all courses")
            {
                num = generator.Next(0, courses.Count);
                course = courses[num];
            }
            return course.Id;
        }
    }
}
