using System.ComponentModel;

namespace VotingApp.Models.RequestModels
{
    public class PositionRequestModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [DisplayName("Available")]
        public bool IsAvailable { get; set; }
        public Guid ElectionId { get; set; }
        public Guid RuleId { get; set; }
    }
}
