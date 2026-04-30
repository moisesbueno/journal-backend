namespace Journal.Infrastructure.MessageBus.Queues
{
    public static class QueuesName
    {
        public const string JournalQueue = "journal-queue";
        public const string JournalDeadLetter = $"journal-dead-letter-queue";
    }

    public static class ExchangesName
    {
        public const string DeadLetterExchange = "dead.letter.exchange";
    }
}