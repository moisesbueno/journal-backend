using FluentValidation;
using Journal.Application.DTOs;
using Journal.Application.Utils;
using Journal.Domain.Abstractions;
using MediatR;
using UserModel = Journal.Domain.Entities.User;

namespace Journal.Application.User.Commands
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<JwtTokenDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<LoginUserCommand> _validator;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(
            IUnitOfWork unitOfWork, 
            IValidator<LoginUserCommand> validator,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<JwtTokenDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErrorResponse { Message = e.ErrorMessage, Property = e.PropertyName }).ToList();

                return Result<JwtTokenDto>.Failure(errors);
            }

            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return Result<JwtTokenDto>.Failure(new List<ErrorResponse> { new ErrorResponse { Message = "Invalid email or password" } });
            }

            if (!PasswordHasher.VerifyPassword(request.Password, user.Password))
            {
                return Result<JwtTokenDto>.Failure(new List<ErrorResponse> { new ErrorResponse { Message = "Invalid email or password" } });
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return Result<JwtTokenDto>.Success(new JwtTokenDto { Token = token });
        }
    }
}