namespace AxiApi.Exceptions
{
    /// <summary>A Custom Option request that breaks a rule. Rejected = 400 (bad input), Conflict = 409 (clashes with what is stored).</summary>
    public class CommandOptionException : Exception
    {
        public bool IsConflict { get; }

        public CommandOptionException(string message, bool isConflict = false) : base(message)
        {
            IsConflict = isConflict;
        }
    }
}
