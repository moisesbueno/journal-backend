using Journal.Application.DTOs;
using Journal.Application.Utils;
using MediatR;

using UserEntity = Journal.Domain.Entities.User;

namespace Journal.Application.User.Commands
{
    public sealed class CreateUserCommand : IRequest<Result<UserCreatedDto>>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordVerification { get; set; }

        public UserEntity ToEntity()
        {
            return new UserEntity(EmailHasher.Hash(Email), PasswordHasher.HashPassword(Password));
        }
    }
}