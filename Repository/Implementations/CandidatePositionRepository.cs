using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class CandidatePositionRepository : ICandidatePositionRepository
    {
        private readonly ApplicationDbContext _context;
        public CandidatePositionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(CandidatePosition candidateposition)
        {
            _context.CandidatePositions.Add(candidateposition);
        }

        public bool Exists(Func<CandidatePosition, bool> predicate)
        {
            return _context.CandidatePositions.Include(c => c.Candidate).ThenInclude(c => c.Student).ThenInclude(c => c.Course).Include(c => c.Position).ThenInclude(c => c.Election).Include(c => c.Votes).Any(predicate);
        }

        public CandidatePosition? Get(Expression<Func<CandidatePosition, bool>> predicate)//changes to be made
        {
            var candidateposition = _context.CandidatePositions.Include(c => c.Candidate).ThenInclude(c => c.Student).ThenInclude(c => c.Course).Include(c => c.Position).ThenInclude(c => c.Election).Include(c => c.Votes).FirstOrDefault(predicate);
            return candidateposition;
        }

        public ICollection<CandidatePosition> GetAll()
        {
            var candidatepositions = _context.CandidatePositions.Include(c => c.Candidate).ThenInclude(c => c.Student).ThenInclude(c => c.Course).Include(c => c.Position).ThenInclude(c => c.Election).Include(c => c.Votes).ToList();
            return candidatepositions;
        }

        public ICollection<CandidatePosition> GetAllByIndex(Expression<Func<CandidatePosition, bool>> predicate)
        {
            var candidatepositions = _context.CandidatePositions.Include(c => c.Candidate).ThenInclude(c => c.Student).ThenInclude(c => c.Course).Include(c => c.Position).ThenInclude(c => c.Election).Include(c => c.Votes).Where(predicate).ToList();
            return candidatepositions;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(CandidatePosition candidateposition)
        {
            _context.Update(candidateposition);
        }
    }
}
