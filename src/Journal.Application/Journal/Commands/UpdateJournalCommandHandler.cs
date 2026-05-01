using FluentValidation;
using Journal.Application.DTOs;
using Journal.Application.Utils;
using Journal.Domain.Abstractions;
using MediatR;
using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Application.Journal.Commands
{
    public class UpdateJournalCommandHandler : IRequestHandler<UpdateJournalCommand, Result<JournalEntity>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateJournalCommand> _validator;

        public UpdateJournalCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateJournalCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<JournalEntity>> Handle(UpdateJournalCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErrorResponse { Message = e.ErrorMessage, Property = e.PropertyName }).ToList();

                return Result<JournalEntity>.Failure(errors);
            }

            var currentJournal = await _unitOfWork.JournalRepository.GetByIdAsync(request.Id);

            var journalToUpdate = request.ToEntity();

            currentJournal.Aimscope = journalToUpdate.Aimscope;
            currentJournal.Name = journalToUpdate.Name;

            await _unitOfWork.JournalRepository.UpdateAsync(currentJournal);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<JournalEntity>.Success(journalToUpdate);
        }
    }
}