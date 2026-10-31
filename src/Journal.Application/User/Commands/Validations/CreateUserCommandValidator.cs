using FluentValidation;
using Journal.Application.Utils;
using Journal.Domain.Abstractions;

namespace Journal.Application.User.Commands.Validations
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(c => c.Email)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .EmailAddress()
               .MustAsync(async (email, cancellationToken) =>
                   await _unitOfWork.UserRepository.GetByEmailAsync(EmailHasher.Hash(email)) is null)
               .WithMessage("Email already registered");

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

    public class LoginUserCommandValidator: AbstractValidator<LoginUserCommand>
    {
        
    }
}
