using FluentValidation;
using Journal.Application.Journal.Commands;

namespace Journal.Application.Journal.Commands.Validations
{
    public class AddJournalCommandValidator : AbstractValidator<AddJournalCommand>
    {
        public AddJournalCommandValidator()
        {
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.Issn)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(10000);
        }
    }
}