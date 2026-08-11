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

        public async Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname, bool forceRefresh = false)
        {
            if (string.IsNullOrWhiteSpace(appname))
            {
                _logger.LogWarning("GetCommandConfigsAsync called with empty appname");
                return new List<CommandConfigDTO>();
            }

            _logger.LogInformation("CommandConfigService: Starting load sequence for app: {AppName}, ForceRefresh: {ForceRefresh}", appname, forceRefresh);

            string cacheKey = Keygenerator.GenerateCacheKey(appname, "command_config", "");
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
    }
}
