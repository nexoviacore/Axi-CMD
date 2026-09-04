using ARMCommon.Helpers;
using ARMCommon.Model;
using AxExtend.Interface; 
using AxiApi.DTOs;
using AxiApi.Lib.Utils;
using AxiApi.Interfaces; 
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace AxiApi.Repositories;

public class GrammarRepository : IGrammarRepository
{
    //private readonly string _connectionString;
   
    private readonly ILogger<GrammarRepository> _logger;
    private readonly IAxExtend _axExtend; 

    public GrammarRepository(IAxExtend axExtend, ILogger<GrammarRepository> logger)
    {
        //_connectionString = configuration.GetConnectionString("Postgres")!;

        _axExtend = axExtend; 
        _logger = logger; 
    }

    public async Task<(List<CommandNodeDTO>, List<PromptNodeDTO>)> LoadGrammerAsync(string appname)
    {

        _logger.LogInformation("Load Grammar Called.....");
        _logger.LogInformation("Appname: " + appname);
        Console.WriteLine("Appname: " + appname);




        
        
       

        var commands = new List<CommandNodeDTO>();
        var prompts = new List<PromptNodeDTO>();
        var processedCommandTokens = new HashSet<int>();
        SQLResult sqlResult = new SQLResult(); 

        //await using var con = new NpgsqlConnection(_connectionString);
        //await con.OpenAsync();

        // 1. Single Query (Efficient)
        const string sql = """
            SELECT
                cp.cmdtoken,        
                cp.command_group,   
                cp.command,        
                pr.wordpos,        
                pr.prompt,         
                pr.promptsource,    
                pr.promptparams,    
                pr.promptvalues,
                pr.extraparams
            FROM axi_commands cp
            LEFT JOIN axi_command_prompts pr
                   ON pr.cmdtoken = cp.cmdtoken
            ORDER BY
                cp.cmdtoken,
                pr.wordpos;
        """;
        _logger.LogInformation("Sql Calling...."); 
        _logger.LogInformation(" SELECT cp.cmdtoken, cp.command_group,  cp.command,pr.wordpos,pr.prompt, pr.promptsource,pr.promptparams, pr.promptvalues    FROM axi_commands cp LEFT JOIN axi_command_prompts pr ON pr.cmdtoken = cp.cmdtoken\r\n    ORDER BY\r\n        cp.cmdtoken, pr.wordpos;");

        var sqlType = DbUtils.GetSqlQueryType(sql);

        var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
        
        if (isDbConnected)
        {
            var db = await _axExtend.GetDB();

           

            sqlResult = await db.ExecuteSQLAsync(sql);


        }

       

        _logger.LogInformation("Sql Result: " + JsonConvert.SerializeObject(sqlResult)); 
        Console.WriteLine("Sql Result: " + JsonConvert.SerializeObject(sqlResult));

        //Console.WriteLine(JsonConvert.SerializeObject(sqlResult));

        if (sqlResult != null && sqlResult.data != null && sqlResult.data.Rows.Count > 0)
        {
            
          
            foreach (DataRow row in sqlResult.data.Rows)
            {
              
                int token = SafeGetInt(row["cmdtoken"]) ?? 0;

               
                if (processedCommandTokens.Add(token))
                {
                    commands.Add(new CommandNodeDTO
                    {
                        CmdToken = token,
                        CommandGroup = row["command_group"]?.ToString() ?? "",
                        Command = row["command"]?.ToString() ?? ""
                    });
                }

               
                if (row["wordpos"] != DBNull.Value)
                {
                    prompts.Add(new PromptNodeDTO
                    {
                        CmdToken = token,
                        WordPos = SafeGetInt(row["wordpos"]),
                        Prompt = row["prompt"]?.ToString(),
                        PromptSource = row["promptsource"]?.ToString(),
                        PromptParams = row["promptparams"]?.ToString(),
                        PromptValues = row["promptvalues"]?.ToString(),
                        ExtraParams = row["extraparams"]?.ToString()
                    });
                }
            }
        }

       

        _logger.LogInformation("Commands: " + JsonConvert.SerializeObject(commands));
        Console.WriteLine("Commands: " + JsonConvert.SerializeObject(commands));

        _logger.LogInformation("Prompts: " + JsonConvert.SerializeObject(prompts));
        Console.WriteLine("Prompts: " + JsonConvert.SerializeObject(prompts));


        return (commands, prompts);
    }

   
    private int? SafeGetInt(object value)
    {
        if (value == null) return null;

       
        try
        {
            return Convert.ToInt32(value);
        }
        catch
        {
         
            string strVal = value.ToString();
            if (int.TryParse(strVal, out int result))
            {
                return result;
            }
            return null;
        }
    }
}