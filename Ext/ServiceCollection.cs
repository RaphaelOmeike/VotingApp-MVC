using VotingApp.Repository.Implementations;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Implementations;
using VotingApp.Services.Interfaces;

namespace VotingApp.Ext
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<ICandidatePositionRepository, CandidatePositionRepository>()
                .AddScoped<ICandidateRepository, CandidateRepository>()
                .AddScoped<ICourseRepository, CourseRepository>()
                .AddScoped<IElectionRepository, ElectionRepository>()
                .AddScoped<IPositionRepository, PositionRepository>()
                .AddScoped<IRuleRepository, RuleRepository>()
                .AddScoped<ISessionRepository, SessionRepository>()
                .AddScoped<IStudentRepository, StudentRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IVoteCastingInfoRepository, VoteCastingInfoRepository>();
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ICandidatePositionService, CandidatePositionService>()
                .AddScoped<ICandidateService, CandidateService>()
                .AddScoped<ICourseService, CourseService>()
                .AddScoped<IElectionService, ElectionService>()
                .AddScoped<IPositionService, PositionService>()
                .AddScoped<IRuleService, RuleService>()
                .AddScoped<ISessionService, SessionService>()
                .AddScoped<IStudentService, StudentService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IVoteCastingInfoService, VoteCastingInfoService>();
        }
    }
}
