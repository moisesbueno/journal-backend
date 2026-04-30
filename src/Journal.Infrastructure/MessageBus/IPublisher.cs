namespace Journal.Infrastructure.MessageBus
{
    public interface IPublisher<TEntity>
    {
        Task SendMessageAsync(TEntity message, string queueName);
    }
}