using AxiApi.DTOs;
using AxiApi.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AxiApi.Services
{
    public class KeyfieldService : IKeyfieldService
    {
        private readonly IKeyfieldRepository _repository;
        private readonly ILogger<KeyfieldService> _logger;

        public KeyfieldService(IKeyfieldRepository repository, ILogger<KeyfieldService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ApiResponseDTO> SetKeyfieldAsync(string appname, SetKeyfieldRequestDTO requestDTO)
        {
            if (string.IsNullOrWhiteSpace(appname))
                throw new ArgumentException("appname is required.");
            if (string.IsNullOrWhiteSpace(requestDTO.TransId))
                throw new ArgumentException("TransId is required.");
            if (string.IsNullOrWhiteSpace(requestDTO.KeyField))
                throw new ArgumentException("KeyField is required.");
            if (string.IsNullOrWhiteSpace(requestDTO.Username))
                throw new ArgumentException("Username is required.");

            _logger.LogInformation("KeyfieldService: Setting keyfield for TransId: {TransId}, KeyField: {KeyField}, App: {AppName}",
                requestDTO.TransId, requestDTO.KeyField, appname);

            bool result = await _repository.SetKeyfieldAsync(appname, requestDTO);

            return new ApiResponseDTO
            {
                Success = result,
                Message = result ? "Success" : "Failed to set key field.",
                StatusCode = 200
            };
        }
    }
}
