using Journal.Domain.Entities;

namespace Journal.Domain.Abstractions
{
    public interface IQualisRepository
    {
        Task<IEnumerable<Qualis>> ListAll();
    }
}