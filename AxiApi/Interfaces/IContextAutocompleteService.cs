using System;

namespace AxiApi.Interfaces;

public interface IContextAutocompleteService
{
    IReadOnlyList<string> Suggest(string input);
}
