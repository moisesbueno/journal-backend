using Journal.Application.Utils;
using MediatR;

using UserEntity = Journal.Domain.Entities.User;

namespace Journal.Application.User.Commands
{
    public sealed class CreateUserCommand : IRequest<Result<UserEntity>>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordVerification { get; set; }

        public UserEntity ToEntity()
        {
            return new UserEntity(Email, PasswordHasher.HashPassword(Password));
        }
    }
}