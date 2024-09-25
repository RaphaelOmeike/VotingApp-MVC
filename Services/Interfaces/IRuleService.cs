using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Services.Interfaces
{
    public interface IRuleService
    {
        BaseResponse<RuleResponseModel> CreateRule(RuleRequestModel request);
        BaseResponse<RuleResponseModel> GetRule(Guid id);
        BaseResponse<ICollection<RuleResponseModel>> GetAllRules();
        BaseResponse<RuleResponseModel> GetRuleByName(string name);
        BaseResponse<RuleRequestModel> GetUpdateRule(Guid id);
        BaseResponse<RuleResponseModel> UpdateRule(Guid id, RuleRequestModel request);
    }
}
