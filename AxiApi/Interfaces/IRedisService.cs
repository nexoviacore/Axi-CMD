using System;

namespace AxiApi.Interfaces;

public interface IRedisService
{
    // String (JSON blob)
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string key);
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);

    // Hash (field-based)
    Task HashSetAsync(string key, string field, string value);
    Task HashSetAsync(string key, IDictionary<string, string> values);
    Task<string?> HashGetAsync(string key, string field);
    Task<IDictionary<string, string>> HashGetAllAsync(string key);
    Task<bool> HashExistsAsync(string key, string field);
    Task HashRemoveAsync(string key, string field);
}
