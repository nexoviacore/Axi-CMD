namespace AxiApi.Exceptions
{
    public class RedisCacheConnectionException: Exception
    {
        public RedisCacheConnectionException(): base("Unable to connect Redis!")
        {

        }

        public RedisCacheConnectionException(string message) : base(message)
        {

        }
    }
}
