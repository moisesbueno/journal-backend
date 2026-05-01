using FluentValidation;
using Journal.Application.Journal.Commands;

namespace Journal.Application.Journal.Commands.Validations
{
    public class AddJournalCommandValidator : AbstractValidator<AddJournalCommand>
    {
        public AddJournalCommandValidator()
        {
            RuleFor(c => c.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.Content)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(10000);
        }
    }
}