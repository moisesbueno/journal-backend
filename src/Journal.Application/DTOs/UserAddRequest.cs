using Journal.Application.User.Commands;
using Journal.Application.Utils;

namespace Journal.Application.DTOs
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordVerification { get; set; }

        public CreateUserCommand ToCreateUserCommand()
        {
            return new CreateUserCommand()
            {
                Email = Email,
                Password = Password,
                PasswordVerification = PasswordVerification
            };
        }
    }
}
