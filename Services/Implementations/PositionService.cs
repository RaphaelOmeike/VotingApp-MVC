using System.Data;
using VotingApp.Models.Constants;
using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;

namespace VotingApp.Services.Implementations
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly ICandidatePositionRepository _candidatePositionRepository;
        public PositionService(IPositionRepository positionRepository, IStudentRepository studentRepository, IElectionRepository electionRepository, ICandidatePositionRepository candidatePositionRepository)
        {
            _positionRepository = positionRepository;
            _studentRepository = studentRepository;
            _electionRepository = electionRepository;
            _candidatePositionRepository = candidatePositionRepository;
        }

        public BaseResponse<PositionResponseModel> ChangePositionStatus(Guid id)
        {
            var position = _positionRepository.Get(c => c.Id == id);
            if (position == null)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such id does not exist! Position not found!"
                };
            }

            //var electionFinished = DateTime.Now >= position.Election.EndDate;
            //if (electionFinished)
            //{
            //    return new BaseResponse<PositionResponseModel>
            //    {
            //        Message = "Election has already been ended!"
            //    };
            //}//wrong
            var electionOngoing = DateTime.Now >= position.Election.StartDate;
            if (electionOngoing)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such id cannot be updated! Elections for this position have been conducted!"
                };
            }
            position.IsAvailable = !position.IsAvailable;
            _positionRepository.Update(position);
            _positionRepository.Save();
            return new BaseResponse<PositionResponseModel>
            {
                Data = new PositionResponseModel
                {
                    Id = position.Id,
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,
                    ElectionId = position.ElectionId,
                    RuleId = position.RuleId
                },
                Status = true,
                Message = "Position availability status changed!"
            };
        }

        public BaseResponse<PositionResponseModel> CreatePosition(PositionRequestModel request)
        {
            var positionExists = _positionRepository.Get(c => c.Name == request.Name && c.ElectionId == request.ElectionId);
            if (positionExists != null)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such name in that election already exists! Registration failed!"
                };
            }
            var election = _electionRepository.Get(c => c.Id == request.ElectionId);
            if (election == null) 
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Election with such id does not exist! Election not found!"
                };
            }
            var electionOngoing = DateTime.Now >= election.StartDate;
            if (electionOngoing)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such cannot be created! Elections for this position have been conducted!"
                };
            }
            Position position = new Position
            {
                Name = request.Name,
                Description = request.Description,
                IsAvailable = request.IsAvailable,
                ElectionId = request.ElectionId,
                RuleId = request.RuleId
            };
            _positionRepository.Create(position);
            _positionRepository.Save();
            return new BaseResponse<PositionResponseModel>
            {
                Data = new PositionResponseModel
                {
                    Id = position.Id,
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,
                    ElectionId = position.ElectionId,
                    RuleId = position.RuleId
                },
                Status = true,
                Message = "Position created successfully!"
            };
        }

        public BaseResponse<ICollection<PositionResponseModel>> GetAllPositions()
        {
            var positions = _positionRepository.GetAll();
            if (!positions.Any())
            {
                return new BaseResponse<ICollection<PositionResponseModel>>
                {
                    Message = "No Active Positions",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<PositionResponseModel>>
            {
                Data = positions.Select(position => new PositionResponseModel
                {
                    Id = position.Id,
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,
                    ElectionId = position.ElectionId,
                    Election = new ElectionResponseModel
                    {
                        Id = position.Election.Id,
                        Name = position.Election.Name,
                        Description = position.Election.Description,
                        ImageUrl = position.Election.ImageUrl,
                        DateCreated = position.Election.StartDate,
                        StartDate = position.Election.StartDate,
                        EndDate = position.Election.EndDate,
                        SessionId = position.Election.SessionId,
                        Session = new SessionResponseModel
                        {
                            Id = position.Election.Session.Id,
                            Name = position.Election.Session.Name,
                            Description = position.Election.Session.Description,
                        },
                        RuleId = position.Election.RuleId
                    },
                    RuleId = position.RuleId,
                    Rule = new RuleResponseModel
                    {
                        Id = position.Rule.Id,
                        Name = position.Rule.Name,
                        Gender = position.Rule.Gender,
                        CourseId = position.Rule.CourseId,
                        MinCGPA = position.Rule.MinCGPA,
                        MaxLevel = position.Rule.MaxLevel,
                        MinLevel = position.Rule.MinLevel,
                    },
                    Contestants = position.CandidatePositions.Select(candidatePosition => new CandidatePositionResponseModel
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
                            RuleId = candidatePosition.Position.RuleId
                        }
                    }).ToList(),
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public BaseResponse<ICollection<PositionResponseModel>> GetAllPositionsForElection(Guid electionId)
        {
            var election = _electionRepository.Get(c => c.Id == electionId);
            if (election == null)
            {
                return new BaseResponse<ICollection<PositionResponseModel>>
                {
                    Message = "Election with such id does not exist! Election not found!",
                    Data = []
                };
            }
            var positions = _positionRepository.GetAllByIndex(c => c.ElectionId == electionId);
            if (!positions.Any())
            {
                return new BaseResponse<ICollection<PositionResponseModel>>
                {
                    Message = "No Active Positions",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<PositionResponseModel>>
            {
                Data = positions.Select(position => new PositionResponseModel
                {
                    Id = position.Id,
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,
                    ElectionId = position.ElectionId,
                    Election = new ElectionResponseModel
                    {
                        Id = position.Election.Id,
                        Name = position.Election.Name,
                        Description = position.Election.Description,
                        ImageUrl = position.Election.ImageUrl,
                        DateCreated = position.Election.StartDate,
                        StartDate = position.Election.StartDate,
                        EndDate = position.Election.EndDate,
                        SessionId = position.Election.SessionId,
                        RuleId = position.Election.RuleId
                    },
                    RuleId = position.RuleId,
                    Rule = new RuleResponseModel
                    {
                        Id = position.Rule.Id,
                        Name = position.Rule.Name,
                        Gender = position.Rule.Gender,
                        CourseId = position.Rule.CourseId,
                        Course = new CourseResponseModel
                        {
                            Id = position.Rule.Course.Id,
                            Name = position.Rule.Course.Name,
                            Description = position.Rule.Course.Description,
                        },
                        MinCGPA = position.Rule.MinCGPA,
                        MaxLevel = position.Rule.MaxLevel,
                        MinLevel = position.Rule.MinLevel,
                    },
                    Contestants = position.CandidatePositions.Select(candidatePosition => new CandidatePositionResponseModel
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
                            RuleId = candidatePosition.Position.RuleId
                        }
                    }).ToList(),
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public BaseResponse<PositionResponseModel> GetPosition(Guid id)
        {
            var position = _positionRepository.Get(c => c.Id == id);
            if (position == null)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such id does not exist! Position not found!"
                };
            }
            return new BaseResponse<PositionResponseModel>
            {
                Data = new PositionResponseModel
                {
                    Id = position.Id,
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,
                    ElectionId = position.ElectionId,
                    RuleId = position.RuleId
                },
                Status = true,
                Message = "Position found!"
            };
        }

        public BaseResponse<PositionResponseModel> GetPositionByElectionName(string positionName, Guid electionId)
        {
            var position = _positionRepository.Get(c => c.Name == positionName && c.ElectionId == electionId);
            if (position == null)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such name in that election does not exist! Position not found!"
                };
            }
            return new BaseResponse<PositionResponseModel>
            {
                Data = new PositionResponseModel
                {
                    Id = position.Id,
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,
                    ElectionId = position.ElectionId,
                    RuleId = position.RuleId
                },
                Status = true,
                Message = "Position found!"
            };
        }

        public BaseResponse<PositionRequestModel> GetUpdatePosition(Guid id)
        {
            var position = _positionRepository.Get(c => c.Id == id);
            if (position == null)
            {
                return new BaseResponse<PositionRequestModel>
                {
                    Message = "Position with such id does not exist! Position not found!"
                };
            }
            return new BaseResponse<PositionRequestModel>
            {
                Data = new PositionRequestModel
                {
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,
                    ElectionId = position.ElectionId,
                    RuleId = position.RuleId
                },
                Status = true,
                Message = "Position found!"
            };
        }

        public BaseResponse<PositionResponseModel> StudentIsEligible(Guid studentId, Guid positionId)
        {
            var student = _studentRepository.Get(c => c.Id == studentId);
            if (student == null)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Student with such id does not exist! Student not eligible!"
                };
            }
            var position = _positionRepository.Get(c => c.Id == positionId);
            if (position == null)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such id does not exist! Student not eligible!"
                };
            }
            
            if (DateTime.Now >= position.Election.StartDate)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such cannot be created! Elections for this position have been conducted!"
                };
            }
            var contestantExists = _candidatePositionRepository.Get(c => c.Candidate.StudentId == studentId && c.PositionId == positionId);
            if (contestantExists != null)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Candidate already contested! Contest failed!"
                };
            }
            if (position.Rule.Name == RuleConst.NoRule)
                {
                return new BaseResponse<PositionResponseModel>
                {
                    Data = new PositionResponseModel
                    {
                        Id = position.Id,
                        Name = position.Name,
                        Description = position.Description,
                        IsAvailable = position.IsAvailable,
                        ElectionId = position.ElectionId,
                        RuleId = position.RuleId
                    },
                    Status = true,
                    Message = " Student eligible!"
                };
            }
            if (!(student.Level >= position.Rule.MinLevel && student.Level <= position.Rule.MaxLevel) || (student.Gender != position.Rule.Gender && position.Rule.Gender != Models.Enums.Gender.All) || (student.CourseId != position.Rule.CourseId && position.Rule.Course.Name != CourseConst.AllCourses) || student.CGPA < position.Rule.MinCGPA)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = " Student not eligible! Position rule(s) violated!"
                };
            }
            return new BaseResponse<PositionResponseModel>
            {
                Data = new PositionResponseModel
                {
                    Id = position.Id,
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,
                    ElectionId = position.ElectionId,
                    RuleId = position.RuleId
                },
                Status = true,
                Message = "Student eligible! Passed all position criteria"
            };
        }

        public BaseResponse<PositionResponseModel> UpdatePosition(Guid id, PositionRequestModel request)
        {
            var position = _positionRepository.Get(c => c.Id == id);
            if (position == null)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Update failed! Position with such id does not exist!"
                };
            }
            var electionOngoing = DateTime.Now >= position.Election.StartDate;
            if (electionOngoing)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such id cannot be updated! Elections for this position have been conducted!"
                };
            }
            var positionExists = _positionRepository.Exists(c => c.Name == request.Name && c.ElectionId == request.ElectionId && c.Id != position.Id);
            if (positionExists)
            {
                return new BaseResponse<PositionResponseModel>
                {
                    Message = "Position with such name in that election already exists! Registration failed!"
                };
            }
            position.Name = request.Name;
            position.Description = request.Description;
            position.IsAvailable = request.IsAvailable;
            position.ElectionId = request.ElectionId;
            position.RuleId = request.RuleId;
            _positionRepository.Update(position);
            _positionRepository.Save();
            return new BaseResponse<PositionResponseModel>
            {
                Data = new PositionResponseModel
                {
                    Id = position.Id,
                    Name = position.Name,
                    Description = position.Description,
                    IsAvailable = position.IsAvailable,//comment later if possible
                    ElectionId = position.ElectionId,
                    RuleId = position.RuleId
                },
                Status = true,
                Message = "Position updated!"
            };
        }
    }
}
