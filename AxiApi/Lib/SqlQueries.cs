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

        // Paged list of one command's options (AxiCMD Config page). {0} = "" or CommandOptionsSearch, {1} = offset, {2} = page size (both
        // integers checked in C#). OFFSET ... FETCH NEXT works on Postgres and on Oracle 12c and later.
        public const string SelectCommandOptionsPage =
            "SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system, record_source " +
            "FROM axi_command_config " +
            "WHERE LOWER(command) = :command{0} " +
            "ORDER BY LOWER(prompt_options), config_id " +
            "OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY";

        public const string CountCommandOptionsPage =
            "SELECT COUNT(*) AS cnt FROM axi_command_config WHERE LOWER(command) = :command{0}";

        // Name or target contains the term. Three names, not one used three times: Oracle binds by position.
        public const string CommandOptionsSearch =
            " AND (LOWER(prompt_options) LIKE :search1 ESCAPE '\\' OR LOWER(target_url) LIKE :search2 ESCAPE '\\' OR LOWER(prompt_id) LIKE :search3 ESCAPE '\\')";

        // Every Configure and SDK option, active or not (the tab counts)
        public const string CountCommandOptionsByCommand =
            "SELECT LOWER(command) AS command, COUNT(*) AS cnt FROM axi_command_config " +
            "WHERE LOWER(command) IN ('configure', 'sdk') GROUP BY LOWER(command)";

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

        // Edit a Custom Option: the stored row, then the row and (when the name or source changed) both prompt lists, replaced in place.
        // The is_system guard keeps a Built-in Option out of reach even if the checks in code were skipped.
        public const string SelectCommandOptionRow =
            "SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system, record_source " +
            "FROM axi_command_config WHERE config_id = :configid AND LOWER(command) IN ('configure', 'sdk')";

        public const string UpdateCustomCommandOption =
            "UPDATE axi_command_config SET prompt_options = :promptoptions, prompt_id = :promptid, prompt_option_type = :optiontype, " +
            "param_field = :paramfield, target_url = :targeturl, extra_params = :extraparams, record_source = :recordsource " +
            "WHERE config_id = :configid AND (is_system IS NULL OR is_system <> 'T')";

        // {0} = cmdtoken (Configure 4, SDK 7)
        public const string ReplaceCommandOptionNames =
            "UPDATE axi_command_prompts SET promptvalues = :value WHERE cmdtoken = {0} AND wordpos = 2";

        public const string ReplaceCommandOptionSources =
            "UPDATE axi_command_prompts SET promptsource = :value WHERE cmdtoken = {0} AND wordpos = 3";

        public const string CheckTstructPropsExists = 
            "SELECT COUNT(*) FROM axp_tstructprops WHERE LOWER(name) = :transid";

        public const string UpdateTstructPropsKeyfield = 
            "UPDATE axp_tstructprops SET keyfield = :keyfield, userconfigured = 't', updatedon = TO_CHAR(CURRENT_TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS'), updatedby = :updatedby WHERE LOWER(name) = :transid";

        public const string InsertTstructPropsKeyfield = 
            "INSERT INTO axp_tstructprops (name, keyfield, userconfigured, createdon, createdby) VALUES (:name, :keyfield, 't', TO_CHAR(CURRENT_TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS'), :createdby)";
    }
}
