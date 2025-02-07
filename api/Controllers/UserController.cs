using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using api.DTOs;
using api.Interfaces;

namespace api.Controllers 
{
    [ApiController]
    [Route("/api/user-management/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IValidator<RequestUserDTO> _validator;
        private readonly IUserService _userService;
        private string _generalMessage = string.Empty;

        public UserController(
            ILogger<UserController> logger,
            IValidator<RequestUserDTO> validator,
            IUserService userService
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RequestUserDTO requestUserDTO)
        {
            try 
            {
                var validationResult = _validator.Validate(requestUserDTO);
                if (!validationResult.IsValid)
                {
                    return UnprocessableEntity(validationResult.Errors);
                }

                var responseDTO = await _userService.CreateAsync(requestUserDTO);

                _generalMessage = (responseDTO.Message?.Text ?? "");

                return (
                    responseDTO.Succeeded 
                    ? await GetAllAsync()
                    : UnprocessableEntity(responseDTO)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating a user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try 
            {
                var responseDTO = await _userService.GetAllAsync(_generalMessage);
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            try 
            {
                var responseDTO = await _userService.GetByIdAsync(id);

                return (
                    responseDTO.Succeeded
                    ? Ok(responseDTO)
                    : NotFound(responseDTO.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] RequestUserDTO requestUserDTO)
        {
            try 
            {
                var validationResult = _validator.Validate(requestUserDTO);
                if (!validationResult.IsValid)
                {
                    return UnprocessableEntity(validationResult.Errors);
                }

                var responseDTO = await _userService.UpdateAsync(id, requestUserDTO);

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
                _logger.LogError(ex, "Error updating the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            try 
            {
                var responseDTO = await _userService.DeleteAsync(id);

                _generalMessage = (responseDTO.Message?.Text ?? "");

                return (
                    responseDTO.Succeeded 
                    ? await GetAllAsync()
                    : NotFound(responseDTO.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
            }
        }
    }
}