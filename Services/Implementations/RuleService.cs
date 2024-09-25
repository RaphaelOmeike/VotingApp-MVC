using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace VotingApp.Services.Implementations
{
    public class RuleService : IRuleService
    {
        private readonly IRuleRepository _ruleRepository;
        public RuleService(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public BaseResponse<RuleResponseModel> CreateRule(RuleRequestModel request)
        {
            var ruleExists = _ruleRepository.Exists(c => c.Name == request.Name);
            if (ruleExists)
            {
                return new BaseResponse<RuleResponseModel>
                {
                    Message = "Rule with such name already exists! Registration failed!"
                };
            }
            Rule rule = new Rule
            {
                Name = request.Name,
                Gender = request.Gender,
                CourseId = request.CourseId,
                MinCGPA = request.MinCGPA,
                MaxLevel = request.MaxLevel,
                MinLevel = request.MinLevel,
            };
            _ruleRepository.Create(rule);
            _ruleRepository.Save();
            return new BaseResponse<RuleResponseModel>
            {
                Data = new RuleResponseModel
                {
                    Id = rule.Id,
                    Name = rule.Name,
                    Gender = rule.Gender,
                    CourseId = rule.CourseId,
                    MinCGPA = rule.MinCGPA,
                    MaxLevel = rule.MaxLevel,
                    MinLevel = rule.MinLevel,
                },
                Status = true,
                Message = "Rule created successfully!"
            };
        }

        public BaseResponse<ICollection<RuleResponseModel>> GetAllRules()
        {
            var rules = _ruleRepository.GetAll();
            if (!rules.Any())
            {
                return new BaseResponse<ICollection<RuleResponseModel>>
                {
                    Message = "No Active Rules",
                    Data = []
                };

            }
            return new BaseResponse<ICollection<RuleResponseModel>>
            {
                Data = rules.Select(rule => new RuleResponseModel
                {
                    Id = rule.Id,
                    Name = rule.Name,
                    Gender = rule.Gender,
                    CourseId = rule.CourseId,
                    Course = new CourseResponseModel
                    {
                        Id = rule.Course.Id,
                        Name = rule.Course.Name,
                        Description = rule.Course.Description,
                    },
                    MinCGPA = rule.MinCGPA,
                    MaxLevel = rule.MaxLevel,
                    MinLevel = rule.MinLevel,
                    Positions = rule.Positions.Select(position => new PositionResponseModel
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
                            RuleId = position.Election.RuleId,
                        },
                        RuleId = position.RuleId
                    }).ToList(),
                    Elections = rule.Elections.Select(election => new ElectionResponseModel
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

        public BaseResponse<RuleResponseModel> GetRule(Guid id)
        {
            var rule = _ruleRepository.Get(c => c.Id == id);
            if (rule == null)
            {
                return new BaseResponse<RuleResponseModel>
                {
                    Message = "Rule with such id does not exist! Rule not found!"
                };
            }
            return new BaseResponse<RuleResponseModel>
            {
                Data = new RuleResponseModel
                {
                    Id = rule.Id,
                    Name = rule.Name,
                    Gender = rule.Gender,
                    CourseId = rule.CourseId,
                    MinCGPA = rule.MinCGPA,
                    MaxLevel = rule.MaxLevel,
                    MinLevel = rule.MinLevel
                },
                Status = true,
                Message = "Rule found!"
            };
        }

        public BaseResponse<RuleResponseModel> GetRuleByName(string name)
        {
            var rule = _ruleRepository.Get(c => c.Name == name);
            if (rule == null)
            {
                return new BaseResponse<RuleResponseModel>
                {
                    Message = "Rule with such name does not exist! Rule not found!"
                };
            }
            return new BaseResponse<RuleResponseModel>
            {
                Data = new RuleResponseModel
                {
                    Id = rule.Id,
                    Name = rule.Name,
                    Gender = rule.Gender,
                    CourseId = rule.CourseId,
                    MinCGPA = rule.MinCGPA,
                    MaxLevel = rule.MaxLevel,
                    MinLevel = rule.MinLevel
                },
                Status = true,
                Message = "Rule found!"
            };
        }

        public BaseResponse<RuleRequestModel> GetUpdateRule(Guid id)
        {
            var rule = _ruleRepository.Get(c => c.Id == id);
            if (rule == null)
            {
                return new BaseResponse<RuleRequestModel>
                {
                    Message = "Rule with such id does not exist! Rule not found!"
                };
            }
            return new BaseResponse<RuleRequestModel>
            {
                Data = new RuleRequestModel
                {
                    Name = rule.Name,
                    Gender = rule.Gender,
                    CourseId = rule.CourseId,
                    MinCGPA = rule.MinCGPA,
                    MaxLevel = rule.MaxLevel,
                    MinLevel = rule.MinLevel
                },
                Status = true,
                Message = "Rule found!"
            };
        }

        public BaseResponse<RuleResponseModel> UpdateRule(Guid id, RuleRequestModel request)
        {
            var rule = _ruleRepository.Get(c => c.Id == id);
            if (rule == null)
            {
                return new BaseResponse<RuleResponseModel>
                {
                    Message = "Rule with such id does not exist! Rule not found!"
                };
            }
            var electionOngoing = rule.Elections.Any(e => DateTime.Now >= e.StartDate);
            if (electionOngoing)
            {
                return new BaseResponse<RuleResponseModel>
                {
                    Message = "Rule with such id cannot be updated! Elections have been conducted using this rule!"
                };
            }
            var ruleExists = _ruleRepository.Exists(c => c.Name == request.Name && c.Id != rule.Id);
            if (ruleExists)
            {
                return new BaseResponse<RuleResponseModel>
                {
                    Message = "Rule with such name already exists! Registration failed!"
                };
            }
            rule.Name = request.Name;
            rule.Gender = request.Gender;
            rule.CourseId = rule.CourseId;
            rule.MinCGPA = request.MinCGPA;
            rule.MaxLevel = request.MaxLevel;
            rule.MinLevel = request.MinLevel;
            _ruleRepository.Update(rule);
            _ruleRepository.Save();
            return new BaseResponse<RuleResponseModel>
            {
                Data = new RuleResponseModel
                {
                    Id = rule.Id,
                    Name = rule.Name,
                    Gender = rule.Gender,
                    CourseId = rule.CourseId,
                    MinCGPA = rule.MinCGPA,
                    MaxLevel = rule.MaxLevel,
                    MinLevel = rule.MinLevel,
                },
                Status = true,
                Message = "Rule updated successfully!"
            };
        }

    }
}
