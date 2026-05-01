using FluentValidation;
using Journal.Application.DTOs;
using Journal.Application.Utils;
using Journal.Domain.Abstractions;
using MediatR;
using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Application.Journal.Commands
{
    public class AddJournalCommandHandler : IRequestHandler<AddJournalCommand, Result<JournalEntity>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddJournalCommand> _validator;

        public AddJournalCommandHandler(IUnitOfWork unitOfWork, IValidator<AddJournalCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<JournalEntity>> Handle(AddJournalCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErrorResponse { Message = e.ErrorMessage, Property = e.PropertyName }).ToList();

                return Result<JournalEntity>.Failure(errors);
            }

            var newJournal = request.ToEntity();

            await _unitOfWork.JournalRepository.AddAsync(newJournal);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<JournalEntity>.Success(newJournal);
        }
    }
}