using System.Data;
using VotingApp.Models.Constants;
using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;

namespace VotingApp.Services.Implementations
{
    public class ElectionService : IElectionService
    {
        private readonly IElectionRepository _electionRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStudentRepository _studentRepository;
        private readonly ICandidatePositionRepository _candidatePositionRepository;
        private readonly IVoteCastingInfoService _voteService;
        private readonly ICandidatePositionService _candidatePositionService;
        private readonly IPositionRepository _positionRepository;
        private readonly ISessionRepository _sessionRepository;

        public ElectionService(IElectionRepository electionRepository, IWebHostEnvironment webHostEnvironment, IStudentRepository studentRepository, ICandidatePositionRepository candidatePositionRepository, IVoteCastingInfoService voteService, ICandidatePositionService candidatePositionService, IPositionRepository positionRepository, ISessionRepository sessionRepository)
        {
            _electionRepository = electionRepository;
            _webHostEnvironment = webHostEnvironment;
            _studentRepository = studentRepository;
            _candidatePositionRepository = candidatePositionRepository;
            _voteService = voteService;
            _candidatePositionService = candidatePositionService;
            _positionRepository = positionRepository;
            _sessionRepository = sessionRepository;
        }

        public BaseResponse<ElectionResponseModel> CreateElection(ElectionRequestModel request)
        {
            var electionExists = _electionRepository.Exists(c => c.Name == request.Name && c.SessionId == request.SessionId);
            if (electionExists)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Election with such name in that session already exists! Registration failed!"
                };
            }
            var session = _sessionRepository.Get(c => c.Id == request.SessionId);
            if (session == null)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Session with such id does not exist! Session not found!"
                };
            }
            Election election = new Election
            {
                Name = $"{request.Name} | {session.Name}",
                Description = request.Description,
                ImageUrl = GetImageUrl(request.Image),
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsClosed = true,
                SessionId = request.SessionId,
                RuleId = request.RuleId
            };
            _electionRepository.Create(election);
            _electionRepository.Save();
            return new BaseResponse<ElectionResponseModel>
            {
                Data = new ElectionResponseModel
                {
                    Id = election.Id,
                    Name = election.Name,
                    Description = election.Description,
                    ImageUrl = election.ImageUrl,
                    DateCreated = election.StartDate,
                    StartDate = election.StartDate,
                    EndDate = election.EndDate,
                    SessionId = election.SessionId,
                    RuleId = election.RuleId
                },
                Status = true,
                Message = "Election created successfully!"
            };
        }

        public BaseResponse<ElectionResponseModel> EndElection()
        {
            var elections = _electionRepository.GetAll();
            foreach (var election in elections)
            {
                if (DateTime.Now >= election.EndDate)
                {
                    election.IsClosed = true;
                    _electionRepository.Update(election);
                    _electionRepository.Save();
                    var positions = _positionRepository.GetAllByIndex(c =>  c.ElectionId == election.Id);
                    if (positions != null)
                    {
                        foreach (var position in positions)
                        {
                            if (position.IsAvailable)
                            {
                                position.IsAvailable = false;
                                _positionRepository.Update(position);
                                _positionRepository.Save();
                            }
                        }
                    }
                    var contestants = _candidatePositionRepository.GetAllByIndex(c => c.Position.ElectionId == election.Id && c.IsDisqualified == false);
                    foreach(var contestant in contestants)
                    {
                        var contestantVotesResponse = _voteService.GetAllVotesForContestant(contestant.CandidateId, contestant.PositionId);
                        if (contestantVotesResponse.Status)
                        {
                            int voteCount = 0;
                            var contestantVotes = contestantVotesResponse.Data;
                            if (contestantVotes != null)
                            {
                                voteCount = contestantVotes.Votes.Count;
                            }
                            _candidatePositionService.UpdateCandidateVotesNo(contestant.Id, voteCount);
                        }
                    }
                }

            }
            return new BaseResponse<ElectionResponseModel>
            {
                Status = true,
                Message = "Ended pending elections success!"
            };
        }

        public BaseResponse<ElectionResponseModel> StartElection()
        {
            var elections = _electionRepository.GetAll();
            foreach (var election in elections)
            {
                var currentDate = DateTime.Now;
                if (currentDate >= election.StartDate && currentDate < election.EndDate)
                {
                    election.IsClosed = false;
                    _electionRepository.Update(election);
                    _electionRepository.Save();
                }

            }
            return new BaseResponse<ElectionResponseModel>
            {
                Status = true,
                Message = "Starting pending elections success!"
            };
        }
        public BaseResponse<ICollection<ElectionResponseModel>> GetAllElections()
        {
            StartElection();
            EndElection(); ///
            var elections = _electionRepository.GetAll();
            if (!elections.Any())
            {
                return new BaseResponse<ICollection<ElectionResponseModel>>
                {
                    Message = "No Active Elections",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<ElectionResponseModel>>
            {
                Data = elections.Select(election => new ElectionResponseModel
                {
                    Id = election.Id,
                    Name = election.Name,
                    Description = election.Description,
                    ImageUrl = election.ImageUrl,
                    DateCreated = election.StartDate,
                    StartDate = election.StartDate,
                    EndDate = election.EndDate,
                    IsClosed = election.IsClosed,
                    SessionId = election.SessionId,
                    Session = new SessionResponseModel
                    {
                        Id = election.Session.Id,
                        Name = election.Session.Name,
                        Description = election.Session.Description,
                    },
                    RuleId = election.RuleId
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public BaseResponse<ElectionResponseModel> GetElection(Guid id)
        {
            var election = _electionRepository.Get(c => c.Id == id);
            if (election == null)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Election with such id does not exist! Election not found!"
                };
            }
            return new BaseResponse<ElectionResponseModel>
            {
                Data = new ElectionResponseModel
                {
                    Id = election.Id,
                    Name = election.Name,
                    Description = election.Description,
                    ImageUrl = election.ImageUrl,
                    DateCreated = election.StartDate,
                    StartDate = election.StartDate,
                    EndDate = election.EndDate,
                    SessionId = election.SessionId,
                    RuleId = election.RuleId,
                    Positions = election.Positions.Select(position => new PositionResponseModel
                    {
                        Id = position.Id,
                        Name = position.Name,
                        Description = position.Description,
                        IsAvailable = position.IsAvailable,
                        ElectionId = position.ElectionId,
                        RuleId = position.RuleId,
                    }).ToList()
                },
                Status = true,
                Message = "Election found!"
            };
        }

        public BaseResponse<ElectionResponseModel> GetElectionBySessionName(string electionName, string sessionName)
        {
            var election = _electionRepository.Get(c => c.Name == electionName && c.Session.Name == sessionName);
            if (election == null)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Election with such id does not exist! Election not found!"
                };
            }
            return new BaseResponse<ElectionResponseModel>
            {
                Data = new ElectionResponseModel
                {
                    Id = election.Id,
                    Name = election.Name,
                    Description = election.Description,
                    ImageUrl = election.ImageUrl,
                    DateCreated = election.StartDate,
                    StartDate = election.StartDate,
                    EndDate = election.EndDate,
                    SessionId = election.SessionId,
                    RuleId = election.RuleId
                },
                Status = true,
                Message = "Election found!"
            };
        }

        public BaseResponse<ElectionRequestModel> GetUpdateElection(Guid id)
        {
            var election = _electionRepository.Get(c => c.Id == id);
            if (election == null)
            {
                return new BaseResponse<ElectionRequestModel>
                {
                    Message = "Election with such id does not exist! Election not found!"
                };
            }
            return new BaseResponse<ElectionRequestModel>
            {
                Data = new ElectionRequestModel
                {
                    Name = election.Name,
                    Description = election.Description,
                    Image = GetImage(election.ImageUrl),
                    StartDate = election.StartDate,
                    EndDate = election.EndDate,
                    SessionId = election.SessionId,
                    RuleId = election.RuleId
                },
                Status = true,
                Message = "Election found!"
            };
        }

        public BaseResponse<ElectionResponseModel> StudentIsEligible(Guid studentId, Guid electionId)
        {
            var student = _studentRepository.Get(c => c.Id == studentId);
            if (student == null)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Student with such id does not exist! Student not eligible!"
                };
            }
            var election = _electionRepository.Get(c => c.Id == electionId);
            if (election == null)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Election with such id does not exist! Student not eligible!"
                };
            }
            //election ongoing implement and electionfinished
            var currentDate = DateTime.Now;
            if (currentDate < election.StartDate)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Election not yet started!"
                };
            }//refine
            if (currentDate >= election.EndDate)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Election has ended!"
                };
            }
            if (election.Rule.Name == RuleConst.NoRule)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Data = new ElectionResponseModel
                    {
                        Id = election.Id,
                        Name = election.Name,
                        Description = election.Description,
                        ImageUrl = election.ImageUrl,
                        DateCreated = election.StartDate,
                        StartDate = election.StartDate,
                        EndDate = election.EndDate,
                        SessionId = election.SessionId,
                        RuleId = election.RuleId
                    },
                    Status = true,
                    Message = " Student eligible!"
                };
            }
            if (!(student.Level >= election.Rule.MinLevel && student.Level <= election.Rule.MaxLevel) || (student.Gender != election.Rule.Gender && election.Rule.Gender != Models.Enums.Gender.All) || (student.CourseId != election.Rule.CourseId && election.Rule.Course.Name != CourseConst.AllCourses) || student.CGPA < election.Rule.MinCGPA)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = " Student not eligible! Election rule(s) violated!"
                };
            }
            return new BaseResponse<ElectionResponseModel>
            {
                Data = new ElectionResponseModel
                {
                    Id = election.Id,
                    Name = election.Name,
                    Description = election.Description,
                    ImageUrl = election.ImageUrl,
                    DateCreated = election.StartDate,
                    StartDate = election.StartDate,
                    EndDate = election.EndDate,
                    SessionId = election.SessionId,
                    RuleId = election.RuleId
                },
                Status = true,
                Message = "Student eligible! Passed all election criteria"
            };
        }

        public BaseResponse<ElectionResponseModel> UpdateElection(Guid id, ElectionRequestModel request)
        {
            var election = _electionRepository.Get(c => c.Id == id);
            if (election == null)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Update failed! Election with such id does not exist!"
                };
            }
            if (DateTime.Now >= election.StartDate)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Update failed! Election conducted"
                };
            }
            var electionExists = _electionRepository.Exists(c => c.Name == request.Name && c.SessionId == request.SessionId && c.Id != election.Id);
            if (electionExists)
            {
                return new BaseResponse<ElectionResponseModel>
                {
                    Message = "Election with such name in that session already exists! Registration failed!"
                };
            }
            election.Name = request.Name;
            election.Description = request.Description;
            election.ImageUrl = GetImageUrl(request.Image);
            election.StartDate = request.StartDate;
            election.EndDate = request.EndDate;
            election.SessionId = request.SessionId;
            election.RuleId = request.RuleId;
            _electionRepository.Update(election);
            _electionRepository.Save();
            return new BaseResponse<ElectionResponseModel>
            {
                Data = new ElectionResponseModel
                {
                    Id = election.Id,
                    Name = election.Name,
                    Description = election.Description,
                    ImageUrl = election.ImageUrl,
                    DateCreated = election.StartDate,
                    StartDate = election.StartDate,
                    EndDate = election.EndDate,
                    SessionId = election.SessionId,
                    RuleId = election.RuleId
                },
                Status = true,
                Message = "Election updated!"
            };
        }
        private string GetImageUrl(IFormFile image)
        {
            if (image == null)
            {
                return "defaulte_img.jpeg";
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
    }
}


//var votes = _voteCastingInfoRepository.GetAllByIndex(c => c.CandidatePosition.Position.ElectionId == election.Id);
//foreach (var vote in votes)
//{
//    var contestants = _candidatePositionRepository.GetAllByIndex(c => c.Position.ElectionId == election.Id);
//    foreach (var contestant in contestants)
//    {

//    }
//}