using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class RuleRepository : IRuleRepository
    {
        private readonly ApplicationDbContext _context;
        public RuleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Rule rule)
        {
            _context.Rules.Add(rule);
        }

        public bool Exists(Func<Rule, bool> predicate)
        {
            return _context.Rules.Any(predicate);
        }

        public Rule? Get(Expression<Func<Rule, bool>> predicate)
        {
            var rule = _context.Rules.Include(c => c.Positions).Include(c => c.Elections).FirstOrDefault(predicate);
            return rule;
        }

        public ICollection<Rule> GetAll()
        {
            var rules = _context.Rules.Include(c => c.Positions).ThenInclude(c => c.Election).Include(c => c.Elections).Include(c => c.Course).ToList();
            return rules;
        }

        public ICollection<Rule> GetAllByIndex(Expression<Func<Rule, bool>> predicate)
        {
            var rules = _context.Rules.Include(c => c.Positions).Include(c => c.Elections).Where(predicate).ToList();
            return rules;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Rule rule)
        {
            _context.Update(rule);
        }
    }
}
