using System;

namespace AxiApi.DTOs;

public class ValidationResult
{
    public bool IsValid { get; init; }
    public string? Error { get; init; }

    public int CmdToken { get; init; }
    public string? CommandGroup { get; init; }
    public string? Command { get; init; }

    public IReadOnlyDictionary<string, string>? Parameters { get; init; }

    public static ValidationResult Fail(string error) => new() { IsValid = false, Error = error };

    public static ValidationResult Ok(
        int cmdToken,
        string group,
        string command,
         IReadOnlyDictionary<string, string> parameters
    ) =>
        new()
        {
            IsValid = true,
            CmdToken = cmdToken,
            CommandGroup = group,
            Command = command,
            Parameters = parameters,
        };
}
