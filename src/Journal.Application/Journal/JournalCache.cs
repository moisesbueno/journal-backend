using System.Text.Json;
using Journal.Application.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Journal.Application.Journal
{
    public class JournalCache
    {
        private const string KeyPrefix = "journal:by-id:";
        private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(5);
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        private readonly IDistributedCache _cache;
        private readonly ILogger<JournalCache> _logger;

        public JournalCache(IDistributedCache cache, ILogger<JournalCache> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<JournalDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var cacheKey = BuildKey(id);

            try
            {
                var cachedBytes = await _cache.GetAsync(cacheKey, cancellationToken);

                if (cachedBytes is null || cachedBytes.Length == 0)
                    return null;

                return JsonSerializer.Deserialize<JournalDto>(cachedBytes, SerializerOptions);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Failed to read journal cache entry {CacheKey}", cacheKey);

                return null;
            }
        }

        public async Task SetAsync(JournalDto journal, CancellationToken cancellationToken)
        {
            var cacheKey = BuildKey(journal.Id);

            try
            {
                var payload = JsonSerializer.SerializeToUtf8Bytes(journal, SerializerOptions);

                await _cache.SetAsync(
                    cacheKey,
                    payload,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = Ttl
                    },
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Failed to write journal cache entry {CacheKey}", cacheKey);
            }
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            var cacheKey = BuildKey(id);

            try
            {
                await _cache.RemoveAsync(cacheKey, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Failed to remove journal cache entry {CacheKey}", cacheKey);
            }
        }

        private static string BuildKey(Guid id) => $"{KeyPrefix}{id}";
    }
}
