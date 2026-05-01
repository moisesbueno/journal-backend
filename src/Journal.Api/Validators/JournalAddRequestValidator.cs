using FluentValidation;
using Journal.Api.Models;
using Journal.Application.DTOs;
using Journal.Domain.Abstractions;

namespace Journal.Api.Validators
{
    public class JournalAddRequestValidator : AbstractValidator<JournalAddRequest>
    {
        public JournalAddRequestValidator(IQualisRepository qualisRepository)
        {
            var qualis = qualisRepository.ListAll().Result
            .Select(c => c.Description)
            .ToList();

            RuleFor(r => r.Issn)
                .NotEmpty()
                .NotNull();

            RuleFor(r => r.Name)
                .NotEmpty()
                .NotNull();

            RuleFor(r => r.Qualis)
                .NotNull()
                .NotEmpty()
                .Must(qualis.Contains)
                .WithMessage($"Permitted values are {string.Join(",", [.. qualis])}");
        }
    }
}