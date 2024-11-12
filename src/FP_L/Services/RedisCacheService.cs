namespace FP_L.Services
{
    using StackExchange.Redis;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class RedisCacheService : ICacheService
    {
        private const string DURATION_SERIALIZATION = "_Duration";
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        #region ICacheService Members

        public async ValueTask<string?> GetAsync(string key)
        {
            var db = GetDatabase();

            // TTI
            await UpdateExpiration(db, key);

            return await db.StringGetAsync(key);
        }

        public async Task SetAsync(string key, string value, TimeSpan duration)
        {
            var db = GetDatabase();

            // TTI
            await SaveDuration(db, key, duration);

            await db.StringSetAsync(key, value, duration);
        }

        public async ValueTask<bool> RemoveKeyAsync(string key)
        {
            var db = GetDatabase();

            // TTI
            await DeleteDuration(db, key);

            return await db.KeyDeleteAsync(key);
        }

        #endregion

        #region Private Helpers

        private IDatabase GetDatabase(int db = -1)
        {
            return connectionMultiplexer.GetDatabase(db);
        }

        private async Task UpdateExpiration(IDatabase db, string key)
        {
            var duration = await GetDuration(db, key);

            if (duration != TimeSpan.Zero)
            {
                await db.KeyExpireAsync(key, DateTime.UtcNow.Add(duration));
            }
        }
        private async Task SaveDuration(IDatabase db, string key, TimeSpan duration)
        {
            if (duration > TimeSpan.Zero)
            {
                await db.StringSetAsync(key + DURATION_SERIALIZATION, JsonSerializer.Serialize(duration));
            }
        }
        private async Task DeleteDuration(IDatabase db, string key)
        {
            await db.KeyDeleteAsync(key + DURATION_SERIALIZATION);
        }
        private async Task<TimeSpan> GetDuration(IDatabase db, string key)
        {
            var json = await db.StringGetAsync(key + DURATION_SERIALIZATION);

            return string.IsNullOrEmpty(json) ? TimeSpan.Zero : JsonSerializer.Deserialize<TimeSpan>(json);
        }

        #endregion
    }
}
