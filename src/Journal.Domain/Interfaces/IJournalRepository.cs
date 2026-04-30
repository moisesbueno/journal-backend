using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Domain.Abstractions
{
    public interface IJournalRepository
    {
        Task AddAsync(JournalEntity model);

        Task UpdateAsync(JournalEntity journal);

        Task<bool> RemoveAsync(Guid id);

        Task<JournalEntity> GetByIdAsync(Guid id);

        Task<IEnumerable<JournalEntity>> GetAsync(string search, int pageNumber, int pageSize);

        Task<int> CountAsync(string search);
    }
}