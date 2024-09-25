using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface ICourseRepository
    {
        void Create(Course course);
        void Update(Course course);
        Course? Get(Expression<Func<Course, bool>> predicate);
        bool Exists(Func<Course, bool> predicate);
        ICollection<Course> GetAll();
        ICollection<Course> GetAllByIndex(Expression<Func<Course, bool>> predicate);
        void Save();
    }
}
