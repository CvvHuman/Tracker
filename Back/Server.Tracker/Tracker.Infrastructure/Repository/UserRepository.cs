using Microsoft.EntityFrameworkCore;
using Tracker.Application.Abstractions;
using Tracker.Domain.Entities;
using Tracker.Infrastructure.Persistence;

namespace Tracker.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TrackerDbContext _context;

        public UserRepository(TrackerDbContext context) => _context = context;

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> Add(User user, CancellationToken cancellationToken) 
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }
    }

}
