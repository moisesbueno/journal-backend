using FluentValidation;
using Journal.Application.DTOs;
using Journal.Application.Utils;
using Journal.Domain.Abstractions;
using MediatR;
using JournalEntity = Journal.Domain.Entities.Journal;
using Journal.Application.Utils;

namespace Journal.Application.Journal.Commands
{
    public class DeleteJournalCommandHandler : IRequestHandler<DeleteJournalCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteJournalCommand> _validator;

        public DeleteJournalCommandHandler(IUnitOfWork unitOfWork, IValidator<DeleteJournalCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(DeleteJournalCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErrorResponse { Message = e.ErrorMessage, Property = e.PropertyName }).ToList();

                return Result<bool>.Failure(errors);
            }

            var result = await _unitOfWork.JournalRepository.RemoveAsync(request.Id);

            if (!result)
            {
                return Result<bool>.Failure(new List<ErrorResponse> { new ErrorResponse { Message = "Journal not found" } });
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}