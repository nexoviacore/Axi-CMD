using System;
using AxiApi.DTOs;
using AxiApi.Interfaces;

namespace AxiApi.Lib.Utils;

public class CommandHandler : ICommandHandler
{
    public int CmdToken => 1;

    public Task ExecuteAsync(ResolvedCommand command)
    {
        // var name = command.Paramaters[0];

        // Real execution logic here
        // Create form / open page / insert DB record

        return Task.CompletedTask;
    }
}
