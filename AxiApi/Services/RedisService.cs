using System;
using System.Text.Json;
using AxiApi.Interfaces;
using StackExchange.Redis;

namespace AxiApi.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;

    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _db.KeyExistsAsync(key);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);

        if (!value.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(value!);
    }

    public Task<bool> HashExistsAsync(string key, string field)
    {
        throw new NotImplementedException();
    }

    public Task<IDictionary<string, string>> HashGetAllAsync(string key)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> HashGetAsync(string key, string field)
    {
        var value = await _db.HashGetAsync(key, field);
        return value.HasValue ? value.ToString() : null;
    }

    public Task HashRemoveAsync(string key, string field)
    {
        throw new NotImplementedException();
    }

    public Task HashSetAsync(string key, string field, string value)
    {
        return _db.HashSetAsync(key, field, value);
    }

    public Task HashSetAsync(string key, IDictionary<string, string> values)
    {
        var entries = values.Select(v => new HashEntry(v.Key, v.Value)).ToArray();

        return _db.HashSetAsync(key, entries);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry, When.Always);
    }
}
