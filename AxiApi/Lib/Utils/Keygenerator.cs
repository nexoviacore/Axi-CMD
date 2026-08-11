namespace AxiApi.Lib.Utils
{
    public class Keygenerator
    {
        private readonly static string prefixKey = "axi";
        
        private Keygenerator() { }

        
        public static string GenerateCacheKey(string appname,  string type, string username = "")
        {
            string key = prefixKey + "_" + appname + "_" + type + "_" + username;

            return key; 
        }
    }
}
