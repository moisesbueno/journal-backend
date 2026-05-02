using Journal.Application.DTOs;
using Journal.Application.Utils;
using MediatR;

using UserEntity = Journal.Domain.Entities.User;

namespace Journal.Application.User.Commands
{
    public sealed class LoginUserCommand : IRequest<Result<JwtTokenDto>>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}