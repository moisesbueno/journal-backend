using FluentValidation;
using Journal.Api.Models;

namespace Journal.Api.Validators
{
    public class JournalUpdateRequestValidator : AbstractValidator<JournalUpdateRequest>
    {
        public JournalUpdateRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Content)
                .NotEmpty();

            RuleFor(x => x.CreatedAt)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}