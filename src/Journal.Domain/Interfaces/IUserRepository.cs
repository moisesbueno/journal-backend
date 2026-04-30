using Journal.Domain.Entities;

namespace Journal.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);

        Task UpdateAsync(User user);

        Task RemoveAsync(Guid userId);

        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdUserIdAsync(Guid userId);

        Task<User> GetByEmailAsync(string email);
    }
}