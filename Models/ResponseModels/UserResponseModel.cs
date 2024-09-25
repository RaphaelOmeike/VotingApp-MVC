using VotingApp.Models.Enums;

namespace VotingApp.Models.ResponseModels
{
    public class UserResponseModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public string RoleName = null!;
    }
}
