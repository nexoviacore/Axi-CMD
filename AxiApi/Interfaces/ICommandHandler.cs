using System;
using AxiApi.DTOs;

namespace AxiApi.Interfaces;

public interface ICommandHandler
{
    int CmdToken { get; }
    Task ExecuteAsync(ResolvedCommand command);

}
