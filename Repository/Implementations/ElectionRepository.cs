using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class ElectionRepository : IElectionRepository
    {
        private readonly ApplicationDbContext _context;
        public ElectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Election election)
        {
            _context.Elections.Add(election);
        }

        public bool Exists(Func<Election, bool> predicate)
        {
            return _context.Elections.Any(predicate);
        }

        public Election? Get(Expression<Func<Election, bool>> predicate)
        {
            var election = _context.Elections.Include(c => c.Session).Include(c => c.Positions).Include(c => c.Rule).ThenInclude(c => c.Course).FirstOrDefault(predicate);
            return election;
        }

        public ICollection<Election> GetAll()
        {
            var elections = _context.Elections.Include(c => c.Session).Include(c => c.Positions).Include(c => c.Rule).ThenInclude(c => c.Course).ToList();
            return elections;
        }

        public ICollection<Election> GetAllByIndex(Expression<Func<Election, bool>> predicate)
        {
            var elections = _context.Elections.Include(c => c.Session).Include(c => c.Positions).Include(c => c.Rule).ThenInclude(c => c.Course).Where(predicate).ToList();
            return elections;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Election election)
        {
            _context.Update(election);
        }

    }
}
