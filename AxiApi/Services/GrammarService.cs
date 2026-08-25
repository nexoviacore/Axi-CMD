using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AxiApi.DTOs;
using AxiApi.Enums;
using AxiApi.Interfaces;
using AxiApi.Lib;
using Npgsql;
//using StackExchange.Redis;

namespace AxiApi.Services;

public class GrammarService : IGrammarService
{
    private const string GrammarCacheKey = Constants.GrammarCache;

     private readonly IGrammarRepository _grammarRepository;
    private readonly IGrammarBootstrapService _grammarBootstrapService; 
    //private readonly IServiceScopeFactory _serviceScopeFactory;

    //private readonly IRedisService _redisService;
    //private readonly IConnectionMultiplexer _redisMux;
    private readonly ILogger<GrammarService> _logger;
    public GrammarDTO Current { get; set; } = null!;

    public GrammarService(
         IGrammarRepository grammarRepository,
        //IServiceScopeFactory serviceScopeFactory,
        //IRedisService redisService,
        //IConnectionMultiplexer redisMux,
        IGrammarBootstrapService grammarBootstrapService,
        ILogger<GrammarService> logger

    )
    {
         _grammarRepository = grammarRepository;
        _grammarBootstrapService = grammarBootstrapService; 
        //_serviceScopeFactory = serviceScopeFactory;
        //_redisService = redisService;
        //_redisMux = redisMux;
        _logger = logger;
    }

    public async Task<object> Get(GrammarView view, bool forceRefresh, string appname)
    {
        try
        {
            //if (forceRefresh)
            //{
            //    _logger.LogInformation(
            //        "Grammar cache empty or refresh requested. Triggering update."
            //    );
            //    await RefreshAsync(appname);
            //}
            switch (view)
            {
                case GrammarView.Metadata:
                    return await _grammarBootstrapService.LoadGrammarAsync(appname, forceRefresh);
                default:
                    throw new InvalidEnumArgumentException("Please enter valid view");
            }
        }
        catch (PostgresException ex)
        {
            throw new Exception(
                "Database error while retriving command metadata. Please check connection.",
                ex
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve Grammar View: {View}", view);
            throw new Exception("An unexpected error occured in the Grammar Service.", ex);
        }
    }

    public void Set(GrammarDTO grammer)
    {
        Current = grammer;
    }

    public async Task RefreshAsync(string appname)
    {
        _logger.LogInformation("Starting Grammar Refresh Cycle...");
        try
        {
            ////try
            ////{
            //    await _redisService.RemoveAsync(GrammarCacheKey);
            ////}
            //catch (RedisException ex)
            //{
            //    _logger.LogError(ex, "Redis cleanup failed, but proceeding with DB fetch.");
            //}

            // var (commands, prompts) = await _grammarRepository.LoadGrammerAsync();

            //using var scope = _serviceScopeFactory.CreateScope();

            //var repo = scope.ServiceProvider.GetRequiredService<IGrammarRepository>();

            var (commands, prompts) = await _grammarRepository.LoadGrammerAsync(appname);

            if (commands == null)
            {
                var msg =
                    "Grammar Repository returned 0 commands. This is likely a DB configuration issue.";
                _logger.LogCritical(msg);
                throw new InvalidOperationException(msg);
            }

            var cacheDto = new GrammarCacheDTO { Commands = commands, Prompts = prompts };
            _logger.LogInformation($"Loaded {commands.Count} and {prompts.Count} Prompts from DB");

            //await _redisService.SetAsync(GrammarCacheKey, cacheDto, TimeSpan.FromHours(12));

            Current = GrammarBuilder.Build(commands, prompts);

            //await _redisMux
            //    .GetSubscriber()
            //    .PublishAsync(RedisChannel.Literal(RedisChannels.GrammarInvalidation), "refresh");

            _logger.LogInformation("Grammar Refresh Cycle Completed Successfully.");
        }
        catch (PostgresException ex)
        {
            _logger.LogError(
                ex,
                $"Database Error during Grammar Refresh. SQL State: {ex.SqlState}"
            );
            throw new Exception("Database failure during grammar load.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical: Unexpected error during Grammar Refresh.");
            throw new Exception("Critical failure during Grammar Refresh", ex);
        }
    }
}
