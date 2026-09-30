using EmployeeManagement.Application.DTOs.Users;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers.V1
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //post: api/v1/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try 
            { 
                var result = await _userService.CreateUserAsync(request);

                return CreatedAtAction(
                    nameof(GetUserById),
                    new { id = result.Id },
                    result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        //get : api/v1/users/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        { 
            var result = await _userService.GetUserByIdAsync(id);
            if (result == null)
            {
                return NotFound(new {
                    message = "User not found."
                });
                 
            }

            return Ok(result);
        }

    }
}
