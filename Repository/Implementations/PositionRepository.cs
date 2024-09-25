using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext _context;
        public PositionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Position position)
        {
            _context.Positions.Add(position);
        }

        public bool Exists(Func<Position, bool> predicate)
        {
            return _context.Positions.Any(predicate);
        }

        public Position? Get(Expression<Func<Position, bool>> predicate)
        {
            var position = _context.Positions
                .Include(c => c.Election)
                .ThenInclude(c => c.Session)
                .Include(c => c.Rule)
                .ThenInclude(c => c.Course)
                .Include(c => c.CandidatePositions)
                .ThenInclude(c => c.Candidate)
                .ThenInclude(c => c.Student)
                .Include(c => c.CandidatePositions)
                .ThenInclude(c => c.Candidate)
                .FirstOrDefault(predicate);
            return position;
        }

        public ICollection<Position> GetAll()
        {
            var positions = _context.Positions
                .Include(c => c.Election)
                .ThenInclude(c => c.Session)
                .Include(c => c.Rule)
                .ThenInclude(c => c.Course)
                .Include(c => c.CandidatePositions)
                .ThenInclude(c => c.Candidate)
                .ThenInclude(c => c.Student)
                .Include(c => c.CandidatePositions)
                .ThenInclude(c => c.Candidate).ToList();
            return positions;
        }

        public ICollection<Position> GetAllByIndex(Expression<Func<Position, bool>> predicate)
        {
            var positions = _context.Positions
                .Include(c => c.Election)
                .ThenInclude(c => c.Session)
                .Include(c => c.Rule)
                .ThenInclude(c => c.Course)
                .Include(c => c.CandidatePositions)
                .ThenInclude(c => c.Candidate)
                .ThenInclude(c => c.Student)
                .Include(c => c.CandidatePositions)
                .ThenInclude(c => c.Candidate)
                .Where(predicate).ToList();
            return positions;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Position position)
        {
            _context.Update(position);
        }
    }
}
