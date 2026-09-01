namespace AxiApi.Lib
{
    public static class SqlQueries
    {
        public const string SelectActiveCommandConfigs = 
            "SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active " +
            "FROM axi_command_config " +
            "WHERE active = 'T' " +
            "ORDER BY command, prompt_options";

        public const string CheckTstructPropsExists = 
            "SELECT COUNT(*) FROM axp_tstructprops WHERE LOWER(name) = :transid";

        public const string UpdateTstructPropsKeyfield = 
            "UPDATE axp_tstructprops SET keyfield = :keyfield, userconfigured = 't', updatedon = TO_CHAR(CURRENT_TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS'), updatedby = :updatedby WHERE LOWER(name) = :transid";

        public const string InsertTstructPropsKeyfield = 
            "INSERT INTO axp_tstructprops (name, keyfield, userconfigured, createdon, createdby) VALUES (:name, :keyfield, 't', TO_CHAR(CURRENT_TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS'), :createdby)";
    }
}
