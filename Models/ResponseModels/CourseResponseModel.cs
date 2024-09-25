namespace VotingApp.Models.ResponseModels
{
    public class CourseResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<StudentResponseModel> Students { get; set; } = [];
        public List<RuleResponseModel> Rules { get; set; } = [];

    }
}
