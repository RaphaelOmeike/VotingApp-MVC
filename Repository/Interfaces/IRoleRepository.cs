using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface IRoleRepository
    {
        void Create(Role role);
        Role? Get(Expression<Func<Role, bool>> predicate);
        bool Exists(Func<Role, bool> predicate);
        ICollection<Role> GetAll();
    }
}
