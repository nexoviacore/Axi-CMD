namespace AxiApi.Lib.Utils
{
    public static class DbUtils
    {
        public static string GetSqlQueryType(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return "UNKNOWN";

            string firstWord = sql.TrimStart().Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.ToUpper();

            return firstWord switch
            {
                "SELECT" => "SELECT",
                "INSERT" => "INSERT",
                "UPDATE" => "UPDATE",
                "DELETE" => "DELETE",
                _ => "OTHER"
            };
        }
    }
}
