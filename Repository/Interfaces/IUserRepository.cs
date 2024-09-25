using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface IUserRepository
    {
        void Create(User user);
        void Update(User user);
        User? Get(Expression<Func<User, bool>> predicate);
        bool Exists(Func<User, bool> predicate);
        ICollection<User> GetAll();
        ICollection<User> GetAllByIndex(Expression<Func<User, bool>> predicate);
        void Save();
    }
}
