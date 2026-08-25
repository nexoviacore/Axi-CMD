using System;
using AxiApi.DTOs;

namespace AxiApi.Interfaces;

public interface ICommandExecutionService
{
    Task ExecuteAsync(ResolvedCommand command);
}
