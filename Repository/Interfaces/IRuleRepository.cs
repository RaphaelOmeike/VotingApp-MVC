using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface IRuleRepository
    {
        void Create(Rule rule);
        void Update(Rule rule);
        Rule? Get(Expression<Func<Rule, bool>> predicate);
        bool Exists(Func<Rule, bool> predicate);
        ICollection<Rule> GetAll();
        ICollection<Rule> GetAllByIndex(Expression<Func<Rule, bool>> predicate);
        void Save();
    }
}
