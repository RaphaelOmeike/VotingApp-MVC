using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ApplicationDbContext _context;
        public SessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Session session)
        {
            _context.Sessions.Add(session);
        }

        public bool Exists(Func<Session, bool> predicate)
        {
            return _context.Sessions.Any(predicate);
        }
        public Session? Get(Expression<Func<Session, bool>> predicate)
        {
            var session = _context.Sessions.Include(c => c.Elections).FirstOrDefault(predicate);
            return session;
        }

        public ICollection<Session> GetAll()
        {
            var sessions = _context.Sessions.Include(c => c.Elections).ToList();
            return sessions;
        }

        public ICollection<Session> GetAllByIndex(Expression<Func<Session, bool>> predicate)
        {
            var sessions = _context.Sessions.Include(c => c.Elections).Where(predicate).ToList();
            return sessions;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Session session)
        {
            _context.Update(session);
        }
    }
}
