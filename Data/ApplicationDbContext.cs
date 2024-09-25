using Microsoft.EntityFrameworkCore;
using VotingApp.Models.Constants;
using VotingApp.Models.Entities;
using VotingApp.Models.Enums;

namespace VotingApp.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidatePosition> CandidatePositions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<VoteCastingInfo> VoteCastingInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CandidatePosition>().HasKey(cp => new
            {
                cp.Id
            });

            modelBuilder.Entity<CandidatePosition>().HasOne(c => c.Candidate).WithMany(cp => cp.CandidatePositions).HasForeignKey(c => c.CandidateId);
            modelBuilder.Entity<CandidatePosition>().HasOne(p => p.Position).WithMany(cp => cp.CandidatePositions).HasForeignKey(p => p.PositionId);

            modelBuilder.Entity<VoteCastingInfo>().HasKey(vc => new
            {
                vc.StudentId,
                vc.CandidatePositionId
            });

            modelBuilder.Entity<VoteCastingInfo>().HasOne(s => s.Student).WithMany(vc => vc.Votes).HasForeignKey(s => s.StudentId);
            modelBuilder.Entity<VoteCastingInfo>().HasOne(cp => cp.CandidatePosition).WithMany(vc => vc.Votes).HasForeignKey(cp => cp.CandidatePositionId);

            modelBuilder.Entity<User>().HasData(
                new User { Email = "admin@gmail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("aasd"), RoleId = "72727393" }
                );
            modelBuilder.Entity<Course>().HasData(
                new Course { Name = "all courses", Description = "represents all the courses" }
                );
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = "72727393", Name = RoleConst.Admin, Description = "the super admin of this application" },
                new Role { Id = "27239479", Name = RoleConst.Student, Description = "the student who votes in this application" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
