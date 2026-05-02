using FluentValidation;
using Journal.Application.DTOs;
using Journal.Application.Utils;
using Journal.Domain.Abstractions;
using MediatR;
using UserModel = Journal.Domain.Entities.User;

namespace Journal.Application.User.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateUserCommand> _validator;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateUserCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<UserModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErrorResponse { Message = e.ErrorMessage, Property = e.PropertyName }).ToList();

                return Result<UserModel>.Failure(errors);
            }

            var newUser = request.ToEntity();

            await _unitOfWork.UserRepository.AddAsync(newUser);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<UserModel>.Success(newUser);
        }
    }
}