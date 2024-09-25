using System.Data;
using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;

namespace VotingApp.Services.Implementations
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public BaseResponse<SessionResponseModel> CreateSession(SessionRequestModel request)
        {
            var sessionExists = _sessionRepository.Exists(c => c.Name == request.Name);
            if (sessionExists)
            {
                return new BaseResponse<SessionResponseModel>
                {
                    Message = "Session with such name already exists! Registration failed!"
                };
            }
            Session session = new Session
            {
                Name = request.Name,
                Description = request.Description
            };
            _sessionRepository.Create(session);
            _sessionRepository.Save();
            return new BaseResponse<SessionResponseModel>
            {
                Data = new SessionResponseModel
                {
                    Id = session.Id,
                    Name = session.Name,
                    Description = session.Description,
                },
                Status = true,
                Message = "Session created successfully!"
            };
        }

        public BaseResponse<ICollection<SessionResponseModel>> GetAllSessions()
        {
            var sessions = _sessionRepository.GetAll();
            if (!sessions.Any())
            {
                return new BaseResponse<ICollection<SessionResponseModel>>
                {
                    Message = "No Active Sessions",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<SessionResponseModel>>
            {
                Data = sessions.Select(session => new SessionResponseModel
                {
                    Id = session.Id,
                    Name = session.Name,
                    Description = session.Description,
                    Elections = session.Elections.Select(election => new ElectionResponseModel
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
                    }).ToList()
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public BaseResponse<SessionResponseModel> GetSession(Guid id)
        {
            var session = _sessionRepository.Get(c => c.Id == id);
            if (session == null)
            {
                return new BaseResponse<SessionResponseModel>
                {
                    Message = "Session with such id does not exist! Session not found!"
                };
            }
            return new BaseResponse<SessionResponseModel>
            {
                Data = new SessionResponseModel
                {
                    Id = session.Id,
                    Name = session.Name,
                    Description = session.Description,
                },
                Status = true,
                Message = "Session found!"
            };
        }

        public BaseResponse<SessionResponseModel> GetSessionByName(string name)
        {
            var session = _sessionRepository.Get(c => c.Name == name);
            if (session == null)
            {
                return new BaseResponse<SessionResponseModel>
                {
                    Message = "Session with such name does not exist! Session not found!"
                };
            }
            return new BaseResponse<SessionResponseModel>
            {
                Data = new SessionResponseModel
                {
                    Id = session.Id,
                    Name = session.Name,
                    Description = session.Description,
                },
                Status = true,
                Message = "Session found!"
            };
        }

        public BaseResponse<SessionRequestModel> GetUpdateSession(Guid id)
        {
            var session = _sessionRepository.Get(c => c.Id == id);
            if (session == null)
            {
                return new BaseResponse<SessionRequestModel>
                {
                    Message = "Session with such id does not exist! Session not found!"
                };
            }
            return new BaseResponse<SessionRequestModel>
            {
                Data = new SessionRequestModel
                {
                    Name = session.Name,
                    Description = session.Description,
                },
                Status = true,
                Message = "Session found!"
            };
        }

        public BaseResponse<SessionResponseModel> UpdateSession(Guid id, SessionRequestModel request)
        {
            var session = _sessionRepository.Get(c => c.Id == id);
            if (session == null)
            {
                return new BaseResponse<SessionResponseModel>
                {
                    Message = "Session with such id does not exist! Session not found!"
                };
            }
            var nameExists = _sessionRepository.Exists(c => c.Name == request.Name && c.Id != session.Id);
            if (nameExists)
            {
                return new BaseResponse<SessionResponseModel>
                {
                    Message = "Name of session already used! Session not updated!"
                };
            }
            session.Name = request.Name;
            session.Description = request.Description;
            _sessionRepository.Update(session);
            _sessionRepository.Save();
            return new BaseResponse<SessionResponseModel>
            {
                Data = new SessionResponseModel
                {
                    Id = session.Id,
                    Name = session.Name,
                    Description = session.Description,
                },
                Status = true,
                Message = "Session updated!"
            };
        }
    }
}
