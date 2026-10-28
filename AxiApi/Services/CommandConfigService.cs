using ARMCommon.Interface;
using AxExtend.Interface;
using AxiApi.DTOs;
using AxiApi.Interfaces;
using AxiApi.Lib.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AxiApi.Services
{
    public class CommandConfigService : ICommandConfigService
    {
        private readonly ICommandConfigRepository _repository;
        private readonly IAxExtend _axExtend;
        private readonly ILogger<CommandConfigService> _logger;

        public CommandConfigService(
            ICommandConfigRepository repository,
            IAxExtend axExtend,
            ILogger<CommandConfigService> logger)
        {
            _repository = repository;
            _axExtend = axExtend;
            _logger = logger;
        }

        public async Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname, string username, bool forceRefresh = false)
        {
            if (string.IsNullOrWhiteSpace(appname) || string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("GetCommandConfigsAsync called with empty appname or username");
                return new List<CommandConfigDTO>();
            }

            _logger.LogInformation("CommandConfigService: Starting load sequence for app: {AppName}, User: {Username}, ForceRefresh: {ForceRefresh}", appname, username, forceRefresh);

            string cacheKey = Keygenerator.GenerateCacheKey(appname, "command_config", username);
            IRedisCacheHelper? redisCache = null;
            var isRedisConnected = await _axExtend.OpenRedisConnectionAsync(appname);

            if (isRedisConnected)
            {
                try
                {
                    redisCache = await _axExtend.GetRedis();
                    if (forceRefresh)
                    {
                        _logger.LogInformation("CommandConfigService: Force refresh requested, clearing Redis cache key: {CacheKey}", cacheKey);
                        await redisCache.KeysDeleteAsync(new RedisKey[] { cacheKey });
                    }
                    else
                    {
                        string cachedString = await redisCache.StringGetAsync(cacheKey);
                        if (!string.IsNullOrEmpty(cachedString))
                        {
                            var cachedList = JsonConvert.DeserializeObject<List<CommandConfigDTO>>(cachedString);
                            if (cachedList != null && cachedList.Count > 0)
                            {
                                _logger.LogInformation("CommandConfigService: Redis Cache Hit with {Count} records for app: {AppName}", cachedList.Count, appname);
                                return cachedList;
                            }
                        }
                    }
                }
                catch (RedisException ex)
                {
                    _logger.LogWarning(ex, "CommandConfigService: Redis operation failed. Falling back to database.");
                }
            }

            _logger.LogInformation("CommandConfigService: Fetching command configs from database for app: {AppName}", appname);
            var configs = await _repository.GetCommandConfigsAsync(appname);

            if (configs != null && configs.Count > 0 && isRedisConnected && redisCache != null)
            {
                try
                {
                    await redisCache.StringSetAsync(cacheKey, JsonConvert.SerializeObject(configs));
                    _logger.LogInformation("CommandConfigService: Saved {Count} command configs to Redis cache for key: {CacheKey}", configs.Count, cacheKey);
                }
                catch (RedisException ex)
                {
                    _logger.LogWarning(ex, "CommandConfigService: Failed to write command configs to Redis cache.");
                }
            }

            return configs ?? new List<CommandConfigDTO>();
        }

        // Not cached: the Config page must always show what is in the table
        public async Task<List<CommandConfigOptionDTO>> GetAllCommandOptionsAsync(string appname)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return new List<CommandConfigOptionDTO>();
            return await _repository.GetAllCommandOptionsAsync(appname);
        }

        public async Task<bool> SetCommandOptionActiveAsync(string appname, string username, string configId, bool active)
        {
            var found = await _repository.SetCommandOptionActiveAsync(appname, configId, active);
            if (!found) return false;

            await ClearCachesAsync(appname, username, includePrompts: false);
            return true;
        }

        public async Task<CommandConfigOptionDTO> CreateCommandOptionAsync(string appname, CreateCommandOptionRequestDTO request)
        {
            var created = await _repository.CreateCommandOptionAsync(appname, request);
            // A new option also changes the prompt lists, which the palette reads from the shared grammar cache
            await ClearCachesAsync(appname, request.Username, includePrompts: true);
            return created;
        }

        // Command-config key GetCommandConfigsAsync reads, so the next GET command-config is served fresh from the table.
        // Other users keep their own cached copy until they press Refresh.
        private async Task ClearCachesAsync(string appname, string username, bool includePrompts)
        {
            var keys = new List<RedisKey> { Keygenerator.GenerateCacheKey(appname, "command_config", username) };
            if (includePrompts) keys.Add(Keygenerator.GenerateCacheKey(appname, "", "commands"));
            try
            {
                if (await _axExtend.OpenRedisConnectionAsync(appname))
                {
                    var redisCache = await _axExtend.GetRedis();
                    await redisCache.KeysDeleteAsync(keys.ToArray());
                }
            }
            catch (RedisException ex)
            {
                _logger.LogWarning(ex, "CommandConfigService: Could not clear Redis cache keys {CacheKeys} after a save.", string.Join(", ", keys));
            }
        }
    }
}
