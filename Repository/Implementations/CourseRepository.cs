using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Course course)
        {
            _context.Courses.Add(course);
        }

        public bool Exists(Func<Course, bool> predicate)
        {
            return _context.Courses.Any(predicate);
        }
        public Course? Get(Expression<Func<Course, bool>> predicate)
        {
            var course = _context.Courses.Include(c => c.Students).Include(c => c.Rules).FirstOrDefault(predicate);
            return course;
        }

        public ICollection<Course> GetAll()
        {
            var courses = _context.Courses.Include(c => c.Students).Include(c => c.Rules).ToList();
            return courses;
        }

        public ICollection<Course> GetAllByIndex(Expression<Func<Course, bool>> predicate)
        {
            var courses = _context.Courses.Include(c => c.Students).Include(c => c.Rules).Where(predicate).ToList();
            return courses;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Course course)
        {
            _context.Update(course);
        }
    }
}
