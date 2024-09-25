using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Role role)
        {
            _context.Roles.Add(role);
        }

        public bool Exists(Func<Role, bool> predicate)
        {
            return _context.Roles.Any(predicate);
        }

        public Role? Get(Expression<Func<Role, bool>> predicate)
        {
            var role = _context.Roles.FirstOrDefault(predicate);
            return role;
        }

        public ICollection<Role> GetAll()
        {
            var roles = _context.Roles.ToList();
            return roles;
        }
    }
}
