using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface IStudentRepository
    {
        void Create(Student student);
        void Update(Student student);
        Student? Get(Expression<Func<Student, bool>> predicate);
        bool Exists(Func<Student, bool> predicate);
        ICollection<Student> GetAll();
        ICollection<Student> GetAllByIndex(Expression<Func<Student, bool>> predicate);
        void Save();
    }
}
