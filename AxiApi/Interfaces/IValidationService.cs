using System;
using AxiApi.DTOs;

namespace AxiApi.Interfaces;

public interface IValidationService
{
    ValidationResult Validate(string input);
}
