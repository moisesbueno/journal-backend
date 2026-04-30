using Journal.Domain.Abstractions;
using Journal.Domain.Entities;
using Journal.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Journal.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JournalContext _journalContext;

        public UserRepository(JournalContext journalContext)
        {
            _journalContext = journalContext;
        }

        public async Task<User> AddAsync(User user)
        {
            await _journalContext.Users.AddAsync(user);
            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _journalContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _journalContext.Users.FirstAsync(c => c.Email == email);
        }

        public async Task<User> GetByIdUserIdAsync(Guid userId)
        {
            return await _journalContext.Users.FirstAsync(c => c.Id == userId);
        }

        public async Task RemoveAsync(Guid userId)
        {
            var user = await _journalContext.Users.FirstAsync(c => c.Id == userId);
            _journalContext.Users.Remove(user);
        }

        public async Task UpdateAsync(User user)
        {
            _journalContext.Users.Update(user);
            await Task.CompletedTask;
        }
    }
}