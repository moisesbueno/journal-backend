using FluentValidation;

namespace Journal.Application.User.Commands.Validations
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {

            RuleFor(c => c.Email)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .EmailAddress();

            RuleFor(c => c.Password)
                .NotNull()
                .MinimumLength(9);

            RuleFor(r => new { r.Password, r.PasswordVerification })
               .Custom((data, context) =>
               {
                   if (data.Password != data.PasswordVerification)
                   {
                       context.AddFailure("PasswordVerification", "PasswordVerifiction incorrect");
                   }
               });
        }
    }
}
