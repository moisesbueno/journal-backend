namespace Journal.Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}