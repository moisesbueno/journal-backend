using FluentValidation;
using Journal.Application.Journal.Commands;
using Journal.Domain.Abstractions;

namespace Journal.Application.Journal.Commands.Validations
{
    public class UpdateJournalCommandValidator : AbstractValidator<UpdateJournalCommand>
    {
        public UpdateJournalCommandValidator()
        {
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.Aimscope)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(10000);
        }
    }
}