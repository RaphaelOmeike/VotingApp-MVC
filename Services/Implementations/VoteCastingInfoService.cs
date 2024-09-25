using VotingApp.Models.Entities;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;

namespace VotingApp.Services.Implementations
{
    public class VoteCastingInfoService : IVoteCastingInfoService
    {
        private readonly IVoteCastingInfoRepository _voteCastingInfoRepository;
        private readonly ICandidatePositionRepository _candidatePositionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly IPositionRepository _positionRepository;
        public VoteCastingInfoService(IVoteCastingInfoRepository voteCastingInfoRepository, ICandidatePositionRepository candidatePositionRepository, IStudentRepository studentRepository, IElectionRepository electionRepository, IPositionRepository positionRepository)
        {
            _voteCastingInfoRepository = voteCastingInfoRepository;
            _candidatePositionRepository = candidatePositionRepository;
            _studentRepository = studentRepository;
            _electionRepository = electionRepository;
            _positionRepository = positionRepository;
        }

        public BaseResponse<VoteResponseModel> CreateVoteCastingInfo(Guid studentId, Guid contestantId)
        {
            var contestant = _candidatePositionRepository.Get(c => c.Id == contestantId && c.IsDisqualified == false);
            if (contestant == null)
            {
                return new BaseResponse<VoteResponseModel>
                {
                    Message = "Contestant does not exist! Vote casting failed!"
                };
            }
            var student = _studentRepository.Get(c => c.Id == studentId);
            if (student == null)
            {
                return new BaseResponse<VoteResponseModel>
                {
                    Message = "Student does not exist! Vote casting failed!"
                };
            }
            if (!student.CanVote)
            {
                return new BaseResponse<VoteResponseModel>
                {
                    Message = "Student not allowed to vote! Vote casting failed!"
                };
            }
            var voteExists = _voteCastingInfoRepository.Exists(c => c.StudentId == studentId && c.CandidatePositionId == contestantId);
            if (voteExists)
            {
                return new BaseResponse<VoteResponseModel>
                {
                    Message = "Vote already made! Vote casting failed!"
                };
            }
            var presentDate = DateTime.Now;
            if (!(presentDate >= contestant.Position.Election.StartDate && presentDate <= contestant.Position.Election.EndDate))
            {
                if (presentDate < contestant.Position.Election.StartDate)
                {
                    return new BaseResponse<VoteResponseModel>
                    {
                        Message = "Election has not started!"
                    };
                }
                return new BaseResponse<VoteResponseModel>
                {
                    Message = "Election has ended!"
                };
            }
            VoteCastingInfo vote = new VoteCastingInfo
            {
                CandidatePositionId = contestant.Id,
                StudentId = studentId
            };
            _voteCastingInfoRepository.Create(vote);
            _voteCastingInfoRepository.Save();
            return new BaseResponse<VoteResponseModel>
            {
                Data = new VoteResponseModel
                {
                    CandidatePositionId = vote.CandidatePositionId,
                    StudentId = vote.StudentId,
                    DateCasted = vote.DateCasted
                },
                Status = true,
                Message = "Vote casted successfully!!"
            };
        }
        public BaseResponse<VoteResponseModel> VoteCastingInfoExists(Guid studentId, Guid positionId)
        {
            var voteExists = _voteCastingInfoRepository.Exists(c => c.StudentId == studentId && c.CandidatePosition.PositionId == positionId);
            
            return new BaseResponse<VoteResponseModel>
            {
                Status = voteExists
            };
        }
        public BaseResponse<ICollection<VoteResponseModel>> GetAll()
        {
            var voteCastingInfos = _voteCastingInfoRepository.GetAll();
            if (!voteCastingInfos.Any())
            {
                return new BaseResponse<ICollection<VoteResponseModel>>
                {
                    Message = "No Active Votes"
                };

            }
            return new BaseResponse<ICollection<VoteResponseModel>>
            {
                Data = voteCastingInfos.Select(vote => new VoteResponseModel
                {
                    CandidatePositionId = vote.CandidatePositionId,
                    StudentId = vote.StudentId,
                    DateCasted = vote.DateCasted
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public BaseResponse<ICollection<VoteResponseModel>> GetAllVotesByStudent(Guid studentId)
        {
            var student = _studentRepository.Get(c => c.Id == studentId);
            if (student == null)
            {
                return new BaseResponse<ICollection<VoteResponseModel>>
                {
                    Message = "Student does not exist! Votes not found!",
                    Data = []
                };
            }
            var voteCastingInfos = _voteCastingInfoRepository.GetAllByIndex(c => c.StudentId == studentId);
            return new BaseResponse<ICollection<VoteResponseModel>>
            {
                Data = voteCastingInfos.Select(vote => new VoteResponseModel
                {
                    CandidatePositionId = vote.CandidatePositionId,
                    CandidatePosition = new CandidatePositionResponseModel
                    {
                        Id = vote.CandidatePosition.Id,
                        Statement = vote.CandidatePosition.Statement,
                        VotesNo = vote.CandidatePosition.VotesNo,
                        Winner = vote.CandidatePosition.Winner,
                        ImageUrl = vote.CandidatePosition.ImageUrl,
                        IsDisqualified = vote.CandidatePosition.IsDisqualified,
                        CandidateId = vote.CandidatePosition.CandidateId,
                        Candidate = new CandidateResponseModel
                        {
                            Id = vote.CandidatePosition.Candidate.Id,
                            StudentId = vote.CandidatePosition.Candidate.StudentId,
                            Student = new StudentResponseModel
                            {
                                Id = vote.CandidatePosition.Candidate.Student.Id,
                                Name = vote.CandidatePosition.Candidate.Student.Name,
                                MatricNo = vote.CandidatePosition.Candidate.Student.MatricNo,
                                Email = vote.CandidatePosition.Candidate.Student.Email,
                                CourseId = vote.CandidatePosition.Candidate.Student.CourseId,
                                Course = new CourseResponseModel
                                {
                                    Id = vote.CandidatePosition.Candidate.Student.Course.Id,
                                    Name = vote.CandidatePosition.Candidate.Student.Course.Name,
                                    Description = vote.CandidatePosition.Candidate.Student.Course.Description
                                },
                                Gender = vote.CandidatePosition.Candidate.Student.Gender,
                                Level = vote.CandidatePosition.Candidate.Student.Level,
                                CGPA = vote.CandidatePosition.Candidate.Student.CGPA,
                                CanVote = vote.CandidatePosition.Candidate.Student.CanVote
                            }
                        },
                        PositionId = vote.CandidatePosition.PositionId, //votes
                        Position = new PositionResponseModel
                        {
                            Id = vote.CandidatePosition.Position.Id,
                            Name = vote.CandidatePosition.Position.Name,
                            Description = vote.CandidatePosition.Position.Description,
                            IsAvailable = vote.CandidatePosition.Position.IsAvailable,
                            ElectionId = vote.CandidatePosition.Position.ElectionId,
                            Election = new ElectionResponseModel
                            {
                                Id = vote.CandidatePosition.Position.Election.Id,
                                Name = vote.CandidatePosition.Position.Election.Name,
                                Description = vote.CandidatePosition.Position.Election.Description,
                                ImageUrl = vote.CandidatePosition.Position.Election.ImageUrl,
                                DateCreated = vote.CandidatePosition.Position.Election.StartDate,
                                StartDate = vote.CandidatePosition.Position.Election.StartDate,
                                EndDate = vote.CandidatePosition.Position.Election.EndDate,
                                SessionId = vote.CandidatePosition.Position.Election.SessionId,
                                RuleId = vote.CandidatePosition.Position.Election.RuleId
                            },
                            RuleId = vote.CandidatePosition.Position.RuleId
                        },
                        Votes = vote.CandidatePosition.Votes.Select(vote => new VoteResponseModel
                        {
                            CandidatePositionId = vote.CandidatePositionId,
                            StudentId = vote.StudentId,
                            DateCasted = vote.DateCasted
                        }).ToList()
                    }
                }).ToList(),
                Status = true,
                Message = "Student exists! Votes found!"
            };
        }
        

        public BaseResponse<CandidatePositionResponseModel> GetAllVotesForContestant(Guid candidateId, Guid positionId)
        {
            var contestant = _candidatePositionRepository.Get(c => c.CandidateId == candidateId && c.PositionId == positionId && c.IsDisqualified == false);
            if (contestant == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Contestant does not exist! Votes not found!"
                };
            }
            return new BaseResponse<CandidatePositionResponseModel>
            {
                Data = new CandidatePositionResponseModel
                {
                    Id = contestant.Id,
                    Statement = contestant.Statement,
                    ImageUrl = contestant.ImageUrl,
                    IsDisqualified = contestant.IsDisqualified,
                    CandidateId = contestant.CandidateId,
                    PositionId = contestant.PositionId,
                    Votes = contestant.Votes.Select(vote => new VoteResponseModel
                    {
                        CandidatePositionId = vote.CandidatePositionId,
                        StudentId = vote.StudentId,
                        DateCasted = vote.DateCasted
                    }).ToList()
                },
                Status = true,
                Message = "Contestant exists! Votes found!"
            };
        }
        public BaseResponse<ICollection<CandidatePositionResponseModel>> GetLiveResults(Guid electionId)
        {
            //refine this method and it's logic
            var election = _electionRepository.Get(c => c.Id == electionId);
            if (election == null)
            {
                return new BaseResponse<ICollection<CandidatePositionResponseModel>>
                {
                    Message = "Election with such id does not exist! Results not found!",
                    Data = []
                };
            }
            
            if (election.IsClosed && DateTime.Now >= election.EndDate)
            {
                var positions = _positionRepository.GetAllByIndex(c => c.ElectionId == election.Id);
                if (positions == null)
                {
                    return new BaseResponse<ICollection<CandidatePositionResponseModel>>
                    {
                        Message = "No positions found for election! Results not found!",
                        Data = []
                    };
                }
                foreach (var position in positions)
                {
                    GetWinner(election.Id, position.Id);
                }
            }
            var candidatePositions = _candidatePositionRepository.GetAllByIndex(c => c.Position.ElectionId == election.Id && c.IsDisqualified == false);
            if (candidatePositions == null)
            {
                return new BaseResponse<ICollection<CandidatePositionResponseModel>>
                {
                    Message = "No contestants for election! Results not found!",
                    Data = []
                };
            }
            //ef core's power try without statement included
            //candidatePositions = _candidatePositionRepository.GetAllByIndex(c => c.Position.ElectionId == election.Id && c.IsDisqualified == false);
            return new BaseResponse<ICollection<CandidatePositionResponseModel>>
            {
                Data = candidatePositions.Select(candidatePosition => new CandidatePositionResponseModel
                {
                    Id = candidatePosition.Id,
                    Statement = candidatePosition.Statement,
                    VotesNo = candidatePosition.VotesNo,
                    Winner = candidatePosition.Winner,
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
                    PositionId = candidatePosition.PositionId, //votes
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
                Message = "Contestants results for election found successfully!"
            };
        }
            //ICollection<CandidatePositionResponseModel> response = [];
            //foreach (var contestant in candidatePositions)
            //{
            //    var votes = GetAllVotesForContestant(contestant.CandidateId, contestant.PositionId);
            //    if (votes.Status)
            //    {
            //        var voteInfo = votes.Data;
            //        if (voteInfo != null)
            //        {
            //            voteInfo.VotesNo = voteInfo.Votes.Count;
            //            response.Add(voteInfo);
            //        }
            //    }
            //}
            //return new BaseResponse<ICollection<CandidatePositionResponseModel>>
            //{
            //    Data = response,
            //    Status = true,
            //    Message = "Contestants for election found!"
            //};
        //}
        public BaseResponse<VoteResponseModel> GetVoteCastingInfo(Guid studentId, Guid candidateId, Guid positionId)
        {
            var vote = _voteCastingInfoRepository.Get(c => c.StudentId == studentId && c.CandidatePosition.CandidateId == candidateId && c.CandidatePosition.PositionId == positionId);

            if (vote == null)
            {
                return new BaseResponse<VoteResponseModel>
                {
                    Message = "Invalid id! Vote does not exist! Vote not found!"
                };
            }
            return new BaseResponse<VoteResponseModel>
            {
                Data = new VoteResponseModel
                {
                    CandidatePositionId = vote.CandidatePositionId,
                    StudentId = vote.StudentId,
                    DateCasted = vote.DateCasted
                },
                Status = true,
                Message = "Contestant found!"
            };
        }
        public BaseResponse<VoteResponseModel> GetVoteCastingInfoById(Guid studentId, Guid candidatePositionId)
        {
            var vote = _voteCastingInfoRepository.Get(c => c.StudentId == studentId && c.CandidatePositionId == candidatePositionId);

            if (vote == null)
            {
                return new BaseResponse<VoteResponseModel>
                {
                    Message = "Invalid id! Vote does not exist! Vote not found!"
                };
            }
            return new BaseResponse<VoteResponseModel>
            {
                Data = new VoteResponseModel
                {   
                    CandidatePositionId = vote.CandidatePositionId,
                    StudentId = vote.StudentId,
                    DateCasted = vote.DateCasted
                },
                Status = true,
                Message = "Contestant found!"
            };
        }
        //public BaseResponse<ICollection<CandidatePositionResponseModel>> GetVotesByStudent(Guid electionId, Guid positionId)

        public BaseResponse<CandidatePositionResponseModel> GetWinner(Guid electionId, Guid positionId)
        {
            var election = _electionRepository.Get(c => c.Id == electionId);
            if (election == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Election with such id does not exist! Election not found!"
                };
            }
            var position = _positionRepository.Get(c => c.Id == positionId);
            if (position == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "Position with such id does not exist! Position not found!"
                };
            }
            var contestants = _candidatePositionRepository.GetAllByIndex(c => c.PositionId == positionId);
            if (contestants == null || !contestants.Any())
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "No active contestants under such position!"
                };
            }
            int maxVote = contestants.Max(c => c.Votes.Count);
            //int maxVotes = results.Max(c => c.VotesNo) ?? 0;

            CandidatePosition? winner = new();
            try
            {
                winner = contestants.SingleOrDefault(c => c.VotesNo == maxVote);

            }
            catch (Exception ex)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "There is a tie between contestants! Re-election!"
                };
            }
            if (winner == null)
            {
                return new BaseResponse<CandidatePositionResponseModel>
                {
                    Message = "There is no winner!"
                };
            }
            if (!winner.Winner && election.IsClosed)
            {
                winner.Winner = true;
                _candidatePositionRepository.Update(winner);
                _candidatePositionRepository.Save();
            }
            return new BaseResponse<CandidatePositionResponseModel>
            {
                Data = new CandidatePositionResponseModel
                {
                    Id = winner.Id,
                    Statement = winner.Statement,
                    ImageUrl = winner.ImageUrl,
                    Winner = true,
                    IsDisqualified = winner.IsDisqualified,
                    CandidateId = winner.CandidateId,
                    PositionId = winner.PositionId,
                    VotesNo = winner.VotesNo,
                    Votes = winner.Votes.Select(vote => new VoteResponseModel
                    {
                        CandidatePositionId = vote.CandidatePositionId,
                        StudentId = vote.StudentId,
                        DateCasted = vote.DateCasted
                    }).ToList()
                },
                Status = true,
                Message = "Contestant exists! Votes found!"
            };
        }
    }
}
