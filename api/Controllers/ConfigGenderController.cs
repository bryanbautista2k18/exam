using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using api.DTOs;
using api.Interfaces;
using api.Mappers;

namespace api.Controllers 
{
    [ApiController]
    [Route("/api/user-management/genders")]
    public class ConfigGenderController : ControllerBase
    {
        private readonly ILogger<ConfigGenderController> _logger;
        private readonly IValidator<RequestConfigGenderDTO> _validator;
        private readonly IConfigGenderService _configGenderService;
        private string _generalMessage = string.Empty;

        public ConfigGenderController(
            ILogger<ConfigGenderController> logger,
             IValidator<RequestConfigGenderDTO> validator,
            IConfigGenderService configGenderService
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _configGenderService = configGenderService ?? throw new ArgumentNullException(nameof(configGenderService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RequestConfigGenderDTO requestConfigGenderDTO)
        {
            try 
            {
                var validationResult = _validator.Validate(requestConfigGenderDTO);
                if (!validationResult.IsValid)
                {
                    return UnprocessableEntity(validationResult.Errors);
                }

                var responseDTO = await _configGenderService.CreateAsync(requestConfigGenderDTO);

                _generalMessage = (responseDTO.Message?.Text ?? "");

                return (
                    responseDTO.Succeeded 
                    ? await GetAllAsync()
                    : UnprocessableEntity(responseDTO)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating a config gender.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try 
            {
                var responseDTO = await _configGenderService.GetAllAsync(_generalMessage);
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all config genders.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try 
            {
                var responseDTO = await _configGenderService.GetByIdAsync(id);

                return (
                    responseDTO.Succeeded
                    ? Ok(responseDTO)
                    : NotFound(responseDTO.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting the config gender.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] RequestConfigGenderDTO requestConfigGenderDTO)
        {
            try 
            {
                var validationResult = _validator.Validate(requestConfigGenderDTO);
                if (!validationResult.IsValid)
                {
                    return UnprocessableEntity(validationResult.Errors);
                }

                var responseDTO = await _configGenderService.UpdateAsync(id, requestConfigGenderDTO);

                _generalMessage = (responseDTO.Message?.Text ?? "");

                return (
                    responseDTO.Succeeded
                    ? await GetAllAsync()
                    : HttpContext.Response.StatusCode switch
                    {
                        StatusCodes.Status404NotFound => NotFound(responseDTO.Message),
                        StatusCodes.Status422UnprocessableEntity => UnprocessableEntity(responseDTO),
                        StatusCodes.Status400BadRequest => BadRequest(responseDTO),
                        _ => Problem(statusCode: (int?)HttpContext.Response.StatusCode)
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating the config gender.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try 
            {
                var responseDTO = await _configGenderService.DeleteAsync(id);

                _generalMessage = (responseDTO.Message?.Text ?? "");

                return (
                    responseDTO.Succeeded 
                    ? await GetAllAsync()
                    : NotFound(responseDTO.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting the config gender.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }
    }
}