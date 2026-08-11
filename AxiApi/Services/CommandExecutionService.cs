using System;
using AxiApi.DTOs;
using AxiApi.Interfaces;

namespace AxiApi.Services;

public class CommandExecutionService : ICommandExecutionService
{
    private readonly IDictionary<int, ICommandHandler> _handlers;

    public CommandExecutionService(IEnumerable<ICommandHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.CmdToken);
    }

    public async Task ExecuteAsync(ResolvedCommand command)
    {
        if (!_handlers.TryGetValue(command.CmdToken, out var handler))
            throw new InvalidOperationException(
                $"No handler registered for CmdToken {command.CmdToken}"
            );

        await handler.ExecuteAsync(command);
    }
}
