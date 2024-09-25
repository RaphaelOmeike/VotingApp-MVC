using System.ComponentModel.DataAnnotations;

namespace VotingApp.Models.RequestModels
{
    public class UpdateUserRequestModel
    {
        public string Password { get; set; } = null!;
        [Compare("Password")]
        public string CPassword { get; set; } = null!;
    }
}
