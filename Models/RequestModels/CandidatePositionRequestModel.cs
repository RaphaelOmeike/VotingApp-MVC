using System.ComponentModel;
using VotingApp.Models.Entities;

namespace VotingApp.Models.RequestModels
{
    public class CandidatePositionRequestModel
    {
        public string? Statement { get; set; }
        public IFormFile Image { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Guid PositionId { get; set; }
    }
}
