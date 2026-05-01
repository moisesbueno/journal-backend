using Journal.Domain.Abstractions;
using Journal.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Journal.Infrastructure.Persistence.Repositories
{
    public class JournalRepository : IJournalRepository
    {

        private readonly JournalContext _journalContext;

        public JournalRepository(JournalContext journalContext)
        {
            _journalContext = journalContext;
        }
        public async Task AddAsync(Domain.Entities.Journal model)
        {
            await _journalContext.Journals.AddAsync(model);
        }

        public async Task<int> CountAsync(string search)
        {
            var query = _journalContext.Journals
                                       .AsNoTracking()
                                       .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            var total = await query.CountAsync();

            return total;
        }
        
        public async Task<IEnumerable<Domain.Entities.Journal>> GetAsync(string search, int pageNumber, int pageSize)
        {
            var query = _journalContext.Journals
                                       .AsNoTracking()
                                       .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            var result = await query.Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            return result;
        }

        public async Task<Domain.Entities.Journal> GetByIdAsync(Guid id)
        {
            return await _journalContext.Journals
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var journal = await _journalContext.Journals.FindAsync(id);

            if (journal is null) return false;
            _journalContext.Journals.Remove(journal);
            return true;

        }

        public async Task UpdateAsync(Domain.Entities.Journal journal)
        {
            _journalContext.Journals.Update(journal);
        }
    }
}