using VotingApp.Models.ResponseModels;
using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;
using static System.Collections.Specialized.BitVector32;
using VotingApp.Models.Constants;

namespace VotingApp.Services.Implementations
{
    public class CandidatePositionService : ICandidatePositionService
    {
        private readonly ICandidatePositionRepository _candidatePositionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICandidateRepository _candidateRepository;
        public CandidatePositionService(ICandidatePositionRepository candidatePositionRepository, IWebHostEnvironment webHostEnvironment, IUserRepository userRepository, ICandidateRepository candidateRepository)
        {
            _candidatePositionRepository = candidatePositionRepository;
            _webHostEnvironment = webHostEnvironment;
            _userRepository = userRepository;
            _candidateRepository = candidateRepository;
        }

        public BaseResponse<CandidatePositionResponseModel> CreateCandidatePosition(CandidatePositionRequestModel request)
        {
            var candidatePositionExists = _candidatePositionRepository.Exists(c => c.Candidate.StudentId == request.StudentId && c.PositionId == request.PositionId);
            if (candidatePositionExists)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Contestant already exists! Registration failed!"
                };
            }
            var candidateExists = _candidateRepository.Get(c => c.StudentId == request.StudentId);
            var candidateId = candidateExists?.Id;
            if (candidateExists == null)
            {
                Candidate candidate = new Candidate
                {
                    StudentId = request.StudentId,
                };
                _candidateRepository.Create(candidate);
                _candidateRepository.Save();
                candidateId = candidate.Id;
            }
            CandidatePosition candidatePosition = new CandidatePosition
            {
                Statement = request.Statement,
                ImageUrl = GetImageUrl(request.Image),
                CandidateId = (Guid)candidateId,
                PositionId = request.PositionId
            };
            _candidatePositionRepository.Create(candidatePosition);
            _candidatePositionRepository.Save();
            return new BaseResponse<CandidatePositionResponseModel>
            {
                Data = new CandidatePositionResponseModel
                {
                    Id = candidatePosition.Id,
                    Statement = candidatePosition.Statement,
                    ImageUrl = candidatePosition.ImageUrl,
                    IsDisqualified = candidatePosition.IsDisqualified,
                    CandidateId = candidatePosition.CandidateId,
                    PositionId = candidatePosition.PositionId
                },
                Status = true,
                Message = "Contestant created successfully!"
            };
            
        }

        public BaseResponse<ICollection<CandidatePositionResponseModel>> GetAllCandidatePositions()
        {
            var candidatePositions = _candidatePositionRepository.GetAll();
            if (!candidatePositions.Any())
            {
                return new BaseResponse<ICollection<CandidatePositionResponseModel>>
                {
                    Message = "No Active Contestants",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<CandidatePositionResponseModel>>
            {
                Data = candidatePositions.Select(candidatePosition => new CandidatePositionResponseModel
                {
                    Id = candidatePosition.Id,
                    Statement = candidatePosition.Statement,
                    VotesNo = candidatePosition.VotesNo,
                    ImageUrl = candidatePosition.ImageUrl,
                    IsDisqualified = candidatePosition.IsDisqualified,
                    CandidateId = candidatePosition.CandidateId,
                    Candidate = new CandidateResponseModel
                    {
                        Id = candidatePosition.Candidate.Id,
                        StudentId = candidatePosition.Candidate.StudentId,
                        Student = new StudentResponseModel
                        {
                            Id = candidatePosition.Candidate.Student.Id,
                            Name = candidatePosition.Candidate.Student.Name,
                            MatricNo = candidatePosition.Candidate.Student.MatricNo,
                            Email = candidatePosition.Candidate.Student.Email,
                            CourseId = candidatePosition.Candidate.Student.CourseId,
                            Course = new CourseResponseModel
                            {
                                Id = candidatePosition.Candidate.Student.Course.Id,
                                Name = candidatePosition.Candidate.Student.Course.Name,
                                Description = candidatePosition.Candidate.Student.Course.Description
                            },
                            Gender = candidatePosition.Candidate.Student.Gender,
                            Level = candidatePosition.Candidate.Student.Level,
                            CGPA = candidatePosition.Candidate.Student.CGPA,
                            CanVote = candidatePosition.Candidate.Student.CanVote
                        }
                    },
                    PositionId = candidatePosition.PositionId,
                    Position = new PositionResponseModel
                    {
                        Id = candidatePosition.Position.Id,
                        Name = candidatePosition.Position.Name,
                        Description = candidatePosition.Position.Description,
                        IsAvailable = candidatePosition.Position.IsAvailable,
                        ElectionId = candidatePosition.Position.ElectionId,
                        Election = new ElectionResponseModel
                        {
                            Id = candidatePosition.Position.Election.Id,
                            Name = candidatePosition.Position.Election.Name,
                            Description = candidatePosition.Position.Election.Description,
                            ImageUrl = candidatePosition.Position.Election.ImageUrl,
                            DateCreated = candidatePosition.Position.Election.StartDate,
                            StartDate = candidatePosition.Position.Election.StartDate,
                            EndDate = candidatePosition.Position.Election.EndDate,
                            SessionId = candidatePosition.Position.Election.SessionId,
                            RuleId = candidatePosition.Position.Election.RuleId
                        },
                        RuleId = candidatePosition.Position.RuleId
                    },
                    Winner = candidatePosition.Winner,
                    Votes = candidatePosition.Votes.Select(vote => new VoteResponseModel
                    {
                        CandidatePositionId = vote.CandidatePositionId,
                        StudentId = vote.StudentId,
                        DateCasted = vote.DateCasted
                    }).ToList()
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }
        public BaseResponse<CandidatePositionResponseModel> GetCandidatePositionById(Guid id)
        {
            var candidatePosition = _candidatePositionRepository.Get(c => c.Id == id);
            if (candidatePosition == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Contestant with such id does not exist! Contestant not found!"
                };
            }
            return new BaseResponse<CandidatePositionResponseModel>
            {
                Data = new CandidatePositionResponseModel
                {
                    Id = candidatePosition.Id,
                    Statement = candidatePosition.Statement,
                    VotesNo = candidatePosition.VotesNo,
                    ImageUrl = candidatePosition.ImageUrl,
                    IsDisqualified = candidatePosition.IsDisqualified,
                    CandidateId = candidatePosition.CandidateId,
                    Candidate = new CandidateResponseModel
                    {
                        Id = candidatePosition.Candidate.Id,
                        StudentId = candidatePosition.Candidate.StudentId,
                        Student = new StudentResponseModel
                        {
                            Id = candidatePosition.Candidate.Student.Id,
                            Name = candidatePosition.Candidate.Student.Name,
                            MatricNo = candidatePosition.Candidate.Student.MatricNo,
                            Email = candidatePosition.Candidate.Student.Email,
                            CourseId = candidatePosition.Candidate.Student.CourseId,
                            Course = new CourseResponseModel
                            {
                                Id = candidatePosition.Candidate.Student.Course.Id,
                                Name = candidatePosition.Candidate.Student.Course.Name,
                                Description = candidatePosition.Candidate.Student.Course.Description
                            },
                            Gender = candidatePosition.Candidate.Student.Gender,
                            Level = candidatePosition.Candidate.Student.Level,
                            CGPA = candidatePosition.Candidate.Student.CGPA,
                            CanVote = candidatePosition.Candidate.Student.CanVote
                        }
                    },
                    PositionId = candidatePosition.PositionId,
                    Position = new PositionResponseModel
                    {
                        Id = candidatePosition.Position.Id,
                        Name = candidatePosition.Position.Name,
                        Description = candidatePosition.Position.Description,
                        IsAvailable = candidatePosition.Position.IsAvailable,
                        ElectionId = candidatePosition.Position.ElectionId,
                        Election = new ElectionResponseModel
                        {
                            Id = candidatePosition.Position.Election.Id,
                            Name = candidatePosition.Position.Election.Name,
                            Description = candidatePosition.Position.Election.Description,
                            ImageUrl = candidatePosition.Position.Election.ImageUrl,
                            DateCreated = candidatePosition.Position.Election.StartDate,
                            StartDate = candidatePosition.Position.Election.StartDate,
                            EndDate = candidatePosition.Position.Election.EndDate,
                            SessionId = candidatePosition.Position.Election.SessionId,
                            RuleId = candidatePosition.Position.Election.RuleId
                        },
                        RuleId = candidatePosition.Position.RuleId
                    },
                    Votes = candidatePosition.Votes.Select(vote => new VoteResponseModel
                    {
                        CandidatePositionId = vote.CandidatePositionId,
                        StudentId = vote.StudentId,
                        DateCasted = vote.DateCasted
                    }).ToList(),
                },
                Status = true,
                Message = "Contestant found!"
            };
        }
        public BaseResponse<CandidatePositionResponseModel> GetCandidatePosition(Guid studentId, Guid positionId)
        {
            var candidatePosition = _candidatePositionRepository.Get(c => c.Candidate.StudentId == studentId && c.PositionId == positionId);
            if (candidatePosition == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Contestant with such id does not exist! Contestant not found!"
                };
            }
            return new BaseResponse<CandidatePositionResponseModel>
            {
                Data = new CandidatePositionResponseModel
                {
                    Id = candidatePosition.Id,
                    Statement = candidatePosition.Statement,
                    ImageUrl = candidatePosition.ImageUrl,
                    IsDisqualified = candidatePosition.IsDisqualified,
                    CandidateId = candidatePosition.CandidateId,
                    PositionId = candidatePosition.PositionId
                },
                Status = true,
                Message = "Contestant found!"
            };
        }

        public BaseResponse<CandidatePositionResponseModel> UpdateCandidatePosition(CandidatePositionRequestModel request)//authorize candidate only
        {
            var candidatePosition = _candidatePositionRepository.Get(c => c.Candidate.StudentId == request.StudentId && c.PositionId == request.PositionId);
            if (candidatePosition == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Update failed! Contestant with such id does not exist!"
                };
            }
            if (DateTime.Now >= candidatePosition.Position.Election.StartDate)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Update failed! Election conducted!"
                };
            }
            candidatePosition.Statement = request.Statement;
            candidatePosition.ImageUrl = GetImageUrl(request.Image);

            _candidatePositionRepository.Update(candidatePosition);
            _candidatePositionRepository.Save();
            return new BaseResponse<CandidatePositionResponseModel>
            {
                Data = new CandidatePositionResponseModel
                {
                    Id = candidatePosition.Id,
                    Statement = candidatePosition.Statement,
                    ImageUrl = candidatePosition.ImageUrl,
                    IsDisqualified = candidatePosition.IsDisqualified,
                    DisqualifierId = candidatePosition.DisqualifierId,
                    CandidateId = candidatePosition.CandidateId,
                    PositionId = candidatePosition.PositionId
                },
                Status = true,
                Message = "Contestant updated!"
            };
        }
        public BaseResponse<CandidatePositionResponseModel> UpdateCandidateVotesNo(Guid id, int votesNo)//authorize??not sure
        {
            var candidatePosition = _candidatePositionRepository.Get(c => c.Id == id && c.IsDisqualified == false);
            if (candidatePosition == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Update failed! Contestant with such id does not exist!"
                };
            }
            candidatePosition.VotesNo = votesNo;
            _candidatePositionRepository.Update(candidatePosition);
            _candidatePositionRepository.Save();
            return new BaseResponse<CandidatePositionResponseModel>
            {
                Data = new CandidatePositionResponseModel
                {
                    Id = candidatePosition.Id,
                    Statement = candidatePosition.Statement,
                    VotesNo = candidatePosition.VotesNo,
                    ImageUrl = candidatePosition.ImageUrl,
                    IsDisqualified = candidatePosition.IsDisqualified,
                    DisqualifierId = candidatePosition.DisqualifierId,
                    CandidateId = candidatePosition.CandidateId,
                    PositionId = candidatePosition.PositionId
                },
                Status = true,
                Message = "Contestant votes number updated!"
            };
        }
        private string GetImageUrl(IFormFile image)
        {
            if (image == null)
            {
                return "defaultcp_img.jpeg";
            }
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            string fileName = image.FileName;
            string filePath = Path.Combine(uploadDir, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
            return fileName;
        }

        public BaseResponse<CandidatePositionResponseModel> ChangeContestantStatus(Guid candidatePositionId, Guid userId)//candidate or admin only! don't change either
        {
            var user = _userRepository.Get(c => c.Id == userId);
            if (user == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "User with such id does not exist! User not found!"
                };
            }
            var candidatePosition = _candidatePositionRepository.Get(c => c.Id == candidatePositionId);
            if (candidatePosition == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Contestant with such id does not exist! Contestant not found!"
                };
            }
            if (DateTime.Now >= candidatePosition.Position.Election.StartDate)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Update failed! Election conducted!"
                };
            }
            if (candidatePosition.DisqualifierId == null || candidatePosition.DisqualifierId == candidatePosition.CandidateId || user.Email == NameConst.AdminMail)
            {
                candidatePosition.IsDisqualified = !candidatePosition.IsDisqualified;

                candidatePosition.DisqualifierId = candidatePosition.CandidateId;
                if (user.Email == NameConst.AdminMail)
                {
                    candidatePosition.DisqualifierId = user.Id;
                }
                _candidatePositionRepository.Update(candidatePosition);
                _candidatePositionRepository.Save();
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Data = new CandidatePositionResponseModel
                    {
                        Id = candidatePosition.Id,
                        Statement = candidatePosition.Statement,
                        ImageUrl = candidatePosition.ImageUrl,
                        IsDisqualified = candidatePosition.IsDisqualified,
                        DisqualifierId = userId,
                        CandidateId = candidatePosition.CandidateId,
                        PositionId = candidatePosition.PositionId
                    },
                    Status = true,
                    Message = "Contestant campaign status updated!"
                };
            }
            
            return new BaseResponse<CandidatePositionResponseModel>
            {
                Message = "Not permitted to change status! Contestant status not updated!"
            };
        }

        public BaseResponse<CandidatePositionRequestModel> GetUpdateCandidatePosition(Guid candidatePositionId)
        {
            var candidatePosition = _candidatePositionRepository.Get(c => c.Id == candidatePositionId);
            if (candidatePosition == null)
            {
                return new BaseResponse<CandidatePositionRequestModel>
                {
                    Message = "Contestant with such id does not exist! Contestant not found!"
                };
            }
            return new BaseResponse<CandidatePositionRequestModel>
            {
                Data = new CandidatePositionRequestModel
                {
                    Statement = candidatePosition.Statement,
                    Image = GetImage(candidatePosition.ImageUrl),
                    StudentId = candidatePosition.Candidate.StudentId,
                    PositionId = candidatePosition.PositionId
                },
                Status = true,
                Message = "Contestant found!"
            };
        }
        private IFormFile GetImage(string imgUrl)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imgUrl);
            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            IFormFile formFile = new FormFile(stream, 0, stream.Length, "asd", Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/*"
            };
            return formFile;
        }

        public BaseResponse<ICollection<CandidatePositionResponseModel>> GetAllContestantsForPosition(Guid positionId)
        {
            var candidatePositions = _candidatePositionRepository.GetAllByIndex(c => c.PositionId == positionId && c.IsDisqualified == false);
            if (!candidatePositions.Any())
            {
                return new BaseResponse<ICollection<CandidatePositionResponseModel>>
                {
                    Message = "No Active Contestants",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<CandidatePositionResponseModel>>
            {
                Data = candidatePositions.Select(candidatePosition => new CandidatePositionResponseModel
                {
                    Id = candidatePosition.Id,
                    Statement = candidatePosition.Statement,
                    VotesNo = candidatePosition.VotesNo,
                    ImageUrl = candidatePosition.ImageUrl,
                    IsDisqualified = candidatePosition.IsDisqualified,
                    CandidateId = candidatePosition.CandidateId,
                    Candidate = new CandidateResponseModel
                    {
                        Id = candidatePosition.Candidate.Id,
                        StudentId = candidatePosition.Candidate.StudentId,
                        Student = new StudentResponseModel
                        {
                            Id = candidatePosition.Candidate.Student.Id,
                            Name = candidatePosition.Candidate.Student.Name,
                            MatricNo = candidatePosition.Candidate.Student.MatricNo,
                            Email = candidatePosition.Candidate.Student.Email,
                            CourseId = candidatePosition.Candidate.Student.CourseId,
                            Course = new CourseResponseModel
                            {
                                Id = candidatePosition.Candidate.Student.Course.Id,
                                Name = candidatePosition.Candidate.Student.Course.Name,
                                Description = candidatePosition.Candidate.Student.Course.Description
                            },
                            Gender = candidatePosition.Candidate.Student.Gender,
                            Level = candidatePosition.Candidate.Student.Level,
                            CGPA = candidatePosition.Candidate.Student.CGPA,
                            CanVote = candidatePosition.Candidate.Student.CanVote
                        }
                    },
                    PositionId = candidatePosition.PositionId,
                    Position = new PositionResponseModel
                    {
                        Id = candidatePosition.Position.Id,
                        Name = candidatePosition.Position.Name,
                        Description = candidatePosition.Position.Description,
                        IsAvailable = candidatePosition.Position.IsAvailable,
                        ElectionId = candidatePosition.Position.ElectionId,
                        Election = new ElectionResponseModel
                        {
                            Id = candidatePosition.Position.Election.Id,
                            Name = candidatePosition.Position.Election.Name,
                            Description = candidatePosition.Position.Election.Description,
                            ImageUrl = candidatePosition.Position.Election.ImageUrl,
                            DateCreated = candidatePosition.Position.Election.StartDate,
                            StartDate = candidatePosition.Position.Election.StartDate,
                            EndDate = candidatePosition.Position.Election.EndDate,
                            SessionId = candidatePosition.Position.Election.SessionId,
                            RuleId = candidatePosition.Position.Election.RuleId
                        },
                        RuleId = candidatePosition.Position.RuleId
                    },
                    Votes = candidatePosition.Votes.Select(vote => new VoteResponseModel
                    {
                        CandidatePositionId = vote.CandidatePositionId,
                        StudentId = vote.StudentId,
                        DateCasted = vote.DateCasted
                    }).ToList()
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }
    }
}
