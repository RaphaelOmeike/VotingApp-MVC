using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(User user)
        {
            _context.Users.Add(user);
        }

        public bool Exists(Func<User, bool> predicate)
        {
            return _context.Users.Any(predicate);
        }

        public User? Get(Expression<Func<User, bool>> predicate)
        {
            var user = _context.Users.Include(c => c.Role).FirstOrDefault(predicate);
            return user;
        }

        public ICollection<User> GetAll()
        {
            var users = _context.Users.Include(c => c.Role).ToList();
            return users;
        }

        public ICollection<User> GetAllByIndex(Expression<Func<User, bool>> predicate)
        {
            var users = _context.Users.Include(c => c.Role).Where(predicate).ToList();
            return users;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Update(user);
        }
    }
}
