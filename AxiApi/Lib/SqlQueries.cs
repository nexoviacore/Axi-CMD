namespace AxiApi.Lib
{
    public static class SqlQueries
    {
        public const string SelectActiveCommandConfigs = 
            "SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active " +
            "FROM axi_command_config " +
            "WHERE active = 'T' " +
            "ORDER BY command, prompt_options";

        // Every Configure / SDK option, inactive ones too (the AxiCMD Config page lists them all)
        public const string SelectAllCommandConfigs =
            "SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system, record_source " +
            "FROM axi_command_config " +
            "WHERE LOWER(command) IN ('configure', 'sdk') " +
            "ORDER BY command, prompt_options";

        // Option names (wordpos 2) and record source names (wordpos 3) of Configure (cmdtoken 4) and SDK (cmdtoken 7)
        public const string SelectCommandOptionPrompts =
            "SELECT cmdtoken, wordpos, promptvalues, promptsource " +
            "FROM axi_command_prompts " +
            "WHERE cmdtoken IN (4, 7) AND wordpos IN (2, 3)";

        // Switch one Configure / SDK option on or off. Only the active flag changes; prompt lists are untouched.
        public const string SelectCommandOptionById =
            "SELECT config_id FROM axi_command_config " +
            "WHERE config_id = :configid AND LOWER(command) IN ('configure', 'sdk')";

        public const string UpdateCommandOptionActive =
            "UPDATE axi_command_config SET active = :active WHERE config_id = :configid";

        // Add a Custom Option: ids and names of the command's options (for the unique checks), then the row and both prompt lists
        public const string SelectCommandOptionNames =
            "SELECT config_id, prompt_options FROM axi_command_config WHERE LOWER(command) = :command";

        public const string InsertCustomCommandOption =
            "INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system, record_source) " +
            "VALUES (:configid, :command, :promptoptions, :promptid, :optiontype, :paramfield, :targeturl, :extraparams, 'T', 'F', :recordsource)";

        // {0} = cmdtoken (Configure 4, SDK 7). The suffix is ',' + name / ',' + source: appended in SQL so two admins saving together do not overwrite each other.
        public const string AppendCommandOptionName =
            "UPDATE axi_command_prompts SET promptvalues = promptvalues || :suffix WHERE cmdtoken = {0} AND wordpos = 2";

        public const string AppendCommandOptionSource =
            "UPDATE axi_command_prompts SET promptsource = promptsource || :suffix WHERE cmdtoken = {0} AND wordpos = 3";

        public const string CheckTstructPropsExists = 
            "SELECT COUNT(*) FROM axp_tstructprops WHERE LOWER(name) = :transid";

        public const string UpdateTstructPropsKeyfield = 
            "UPDATE axp_tstructprops SET keyfield = :keyfield, userconfigured = 't', updatedon = TO_CHAR(CURRENT_TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS'), updatedby = :updatedby WHERE LOWER(name) = :transid";

        public const string InsertTstructPropsKeyfield = 
            "INSERT INTO axp_tstructprops (name, keyfield, userconfigured, createdon, createdby) VALUES (:name, :keyfield, 't', TO_CHAR(CURRENT_TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS'), :createdby)";
    }
}
