namespace AxiApi.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(string entityName): base($"{entityName} was not found")
        {

        }

        public NotFoundException(string entityName, string propertyName, string value) : base($"{entityName} with {propertyName}: '{value}' was not found!")
        {

        }
    }
}
