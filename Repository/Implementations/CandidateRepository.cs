using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;
        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
        }

        public bool Exists(Func<Candidate, bool> predicate)
        {
            return _context.Candidates.Any(predicate);
        }

        public Candidate? Get(Expression<Func<Candidate, bool>> predicate)
        {
            var candidate = _context.Candidates.Include(c => c.Student).Include(c => c.CandidatePositions).FirstOrDefault(predicate);
            return candidate;
        }

        public ICollection<Candidate> GetAll()
        {
            var candidates = _context.Candidates.Include(c => c.Student).Include(c => c.CandidatePositions).ThenInclude(c => c.Position).Include(c => c.CandidatePositions).ThenInclude(c => c.Votes).ToList();
            return candidates;
        }

        public ICollection<Candidate> GetAllByIndex(Expression<Func<Candidate, bool>> predicate)
        {
            var candidates = _context.Candidates.Include(c => c.Student).Include(c => c.CandidatePositions).Where(predicate).ToList();
            return candidates;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Candidate candidate)
        {
            _context.Update(candidate);
        }
    }
}
