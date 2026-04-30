using FluentValidation;
using Journal.Application.DTOs;
using Journal.Application.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Journal.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;


        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request.ToCreateUserCommand(), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(new { result.Data });
            }
            else
            {
                return BadRequest(new { errors = result.Errors });
            }
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetUserAsync([FromBody] CreateUserCommand userCommand, CancellationToken cancellationToken)
        //{
        //    var createdUser = await _mediator.Send(userCommand, cancellationToken);
        //    return Ok(createdUser);
        //}



        //[HttpPost("auth")]
        //public async Task<IActionResult> AuthAsync([FromBody] UserAuthRequest userAuthRequest)
        //{
        //    var user = await _userRepository.GetUserByEmailAsync(userAuthRequest.Email);

        //    if (user is null)
        //        return NotFound();

        //    var passWordIsValid = PasswordHasher.VerifyPassword(userAuthRequest.Password, user.Password);

        //    if (passWordIsValid)
        //    {
        //        await _userRepository.UpdatePassWordHashAsync(user.Email, userAuthRequest.Password);
        //        return Ok(user);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}
    }
}