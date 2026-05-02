using FluentValidation;
using Journal.Application.Journal.Commands;

namespace Journal.Application.Journal.Commands.Validations
{
    public class DeleteJournalCommandValidator : AbstractValidator<DeleteJournalCommand>
    {
        public DeleteJournalCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();
        }
    }
}