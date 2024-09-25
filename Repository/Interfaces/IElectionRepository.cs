using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface IElectionRepository
    {
        void Create(Election election);
        void Update(Election election);
        Election? Get(Expression<Func<Election, bool>> predicate);
        bool Exists(Func<Election, bool> predicate);
        ICollection<Election> GetAll();
        ICollection<Election> GetAllByIndex(Expression<Func<Election, bool>> predicate);
        void Save();
    }
}
