namespace AxiApi.Exceptions
{
    /// <summary>A Custom Option request that breaks a rule. Rejected = 400 (bad input), Conflict = 409 (clashes with what is stored).</summary>
    public class CommandOptionException : Exception
    {
        public bool IsConflict { get; }
        /// <summary>Forbidden = 403: the request is well formed but the option may not be changed (Built-in Option route).</summary>
        public bool IsForbidden { get; }

        public CommandOptionException(string message, bool isConflict = false, bool isForbidden = false) : base(message)
        {
            IsConflict = isConflict;
            IsForbidden = isForbidden;
        }
    }
}
