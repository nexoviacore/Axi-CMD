namespace AxiApi.Lib
{
    public static class SqlQueries
    {
        public const string SelectActiveCommandConfigs = 
            "SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active " +
            "FROM axi_command_config " +
            "WHERE active = 'T' " +
            "ORDER BY command, prompt_options";

        public const string SelectAllCommandConfigs = 
            "SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active " +
            "FROM axi_command_config " +
            "ORDER BY command, prompt_options";

        public const string SelectCommandPrompts = 
            "SELECT p.id, p.cmdtoken, COALESCE(NULLIF(c.command, ''), c.command_group, '') AS command, c.command_group, p.wordpos, p.prompt, p.promptsource, p.promptparams, p.promptvalues, p.extraparams, p.requesturl " +
            "FROM axi_command_prompts p " +
            "LEFT JOIN axi_commands c ON p.cmdtoken = c.cmdtoken " +
            "ORDER BY p.cmdtoken, p.wordpos";

        public const string CheckConfigExists = 
            "SELECT COUNT(1) FROM axi_command_config WHERE config_id = @config_id";

        public const string InsertCommandConfig = 
            "INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) " +
            "VALUES (@config_id, @command, @prompt_options, @prompt_id, @prompt_option_type, @param_field, @target_url, @extra_params, @active)";

        public const string UpdateCommandConfig = 
            "UPDATE axi_command_config " +
            "SET command = @command, prompt_options = @prompt_options, prompt_id = @prompt_id, " +
            "prompt_option_type = @prompt_option_type, param_field = @param_field, target_url = @target_url, " +
            "extra_params = @extra_params, active = @active " +
            "WHERE config_id = @config_id";

        public const string DeleteCommandConfig = 
            "DELETE FROM axi_command_config WHERE config_id = @config_id";

        public const string UpdateCommandPrompt = 
            "UPDATE axi_command_prompts " +
            "SET promptvalues = @promptvalues, promptsource = @promptsource " +
            "WHERE cmdtoken = @cmdtoken AND wordpos = @wordpos";
    }
}
