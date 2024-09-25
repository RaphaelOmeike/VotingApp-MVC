using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(Student student)
        {
            _context.Students.Add(student);
        }

        public bool Exists(Func<Student, bool> predicate)
        {
            return _context.Students.Any(predicate);
        }

        public Student? Get(Expression<Func<Student, bool>> predicate)
        {
            var student = _context.Students.Include(s => s.Course).Include(s => s.Votes).ThenInclude(s => s.CandidatePosition).Where(s => s.IsDeleted == false).FirstOrDefault(predicate);
            return student;
        }

        public ICollection<Student> GetAll()
        {
            var students = _context.Students.Include(s => s.Course).Include(s => s.Votes).ThenInclude(s => s.CandidatePosition).ToList();
            return students;
        }

        public ICollection<Student> GetAllByIndex(Expression<Func<Student, bool>> predicate)
        {
            var students = _context.Students.Include(s => s.Course).Include(s => s.Votes).ThenInclude(s => s.CandidatePosition).Where(predicate).ToList();
            return students;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Student student)
        {
            _context.Update(student);
        }
    }
}
