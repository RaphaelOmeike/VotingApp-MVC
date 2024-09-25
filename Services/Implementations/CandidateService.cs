using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;

namespace VotingApp.Services.Implementations
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IStudentRepository _studentRepository;
        public CandidateService(ICandidateRepository candidateRepository, IStudentRepository studentRepository)
        {
            _candidateRepository = candidateRepository;
            _studentRepository = studentRepository;
        }

        public BaseResponse<CandidateResponseModel> CreateCandidate(CandidateRequestModel request)
        {
            var candidateExists = _candidateRepository.Exists(c => c.StudentId == request.StudentId);
            if (candidateExists)
            {
                return new BaseResponse<CandidateResponseModel>
                {
                    Message = "Candidate already exists! Registration failed!"
                };
            }
            Candidate candidate = new Candidate
            {
               StudentId = request.StudentId
            };
            _candidateRepository.Create(candidate);
            _candidateRepository.Save();
            return new BaseResponse<CandidateResponseModel>
            {
                Data = new CandidateResponseModel
                {
                    Id = candidate.Id,
                    StudentId = request.StudentId,
                },
                Status = true,
                Message = "Candidate created successfully!"
            };
        }

        public BaseResponse<ICollection<CandidateResponseModel>> GetAllCandidates()
        {
            var candidates = _candidateRepository.GetAll();
            if (!candidates.Any())
            {
                return new BaseResponse<ICollection<CandidateResponseModel>>
                {
                    Message = "No Active Candidates",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<CandidateResponseModel>>
            {
                Data = candidates.Select(c => new CandidateResponseModel
                {
                    Id = c.Id,
                    StudentId = c.StudentId,
                    Student = new StudentResponseModel
                    {
                        Id = c.Student.Id,
                        Name = c.Student.Name,
                        MatricNo = c.Student.MatricNo,
                        Email = c.Student.Email,
                        CourseId = c.Student.CourseId,
                        Gender = c.Student.Gender,
                        Level = c.Student.Level,
                        CGPA = c.Student.CGPA,
                        CanVote = c.Student.CanVote
                    },
                    CandidatePositions = c.CandidatePositions.Select(candidatePosition => new CandidatePositionResponseModel
                    {
                        Id = candidatePosition.Id,
                        Statement = candidatePosition.Statement,
                        ImageUrl = candidatePosition.ImageUrl,
                        IsDisqualified = candidatePosition.IsDisqualified,
                        CandidateId = candidatePosition.CandidateId,
                        PositionId = candidatePosition.PositionId,
                        Position = new PositionResponseModel
                        {
                            Id = candidatePosition.Position.Id,
                            Name = candidatePosition.Position.Name,
                            Description = candidatePosition.Position.Description,
                            IsAvailable = candidatePosition.Position.IsAvailable,
                            ElectionId = candidatePosition.Position.ElectionId,
                            RuleId = candidatePosition.Position.RuleId
                        },
                        Votes = candidatePosition.Votes.Select(vote => new VoteResponseModel
                        {
                            CandidatePositionId = vote.CandidatePositionId,
                            StudentId = vote.StudentId,
                            DateCasted = vote.DateCasted
                        }).ToList()
                    }).ToList(),
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public BaseResponse<CandidateResponseModel> GetCandidate(Guid id)
        {
            var candidate = _candidateRepository.Get(c => c.Id == id);
            if (candidate == null)
            {
                return new BaseResponse<CandidateResponseModel>
                {
                    Message = "Candidate with such id does not exist! Candidate not found!"
                };
            }
            return new BaseResponse<CandidateResponseModel>
            {
                Data = new CandidateResponseModel
                {
                    Id = candidate.Id,
                    StudentId = candidate.StudentId
                },
                Status = true,
                Message = "Candidate found!"
            };
        }

        public BaseResponse<CandidateResponseModel> GetCandidateByEmail(string email)
        {
            var student = _studentRepository.Get(c => c.Email == email);
            if (student == null)
            {
                return new BaseResponse<CandidateResponseModel>
                {
                    Message = "Student with such email does not exist! Candidate not found!"
                };
            }
            var candidate = _candidateRepository.Get(c => c.Student.Id == student.Id);
            if (candidate == null)
            {
                return new BaseResponse<CandidateResponseModel>
                {
                    Message = "Candidate with such id does not exist! Candidate not found!"
                };
            }
            return new BaseResponse<CandidateResponseModel>
            {
                Data = new CandidateResponseModel
                {
                    Id = candidate.Id,
                    StudentId = candidate.StudentId
                },
                Status = true,
                Message = "Candidate found!"
            };
        }

        public BaseResponse<CandidateResponseModel> GetCandidateByStudentId(Guid id)
        {
            var student = _studentRepository.Get(c => c.Id == id);
            if (student == null)
            {
                return new BaseResponse<CandidateResponseModel>
                {
                    Message = "Student with such id does not exist! Candidate not found!"
                };
            }
            var candidate = _candidateRepository.Get(c => c.Student.Id == id);
            if (candidate == null)
            {
                return new BaseResponse<CandidateResponseModel>
                {
                    Message = "Student with such id does not exist as a candidate! Candidate not found!"
                };
            }
            return new BaseResponse<CandidateResponseModel>
            {
                Data = new CandidateResponseModel
                {
                    Id = candidate.Id,
                    StudentId = candidate.StudentId
                },
                Status = true,
                Message = "Candidate found!"
            };
        }
    }
}

