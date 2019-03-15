using Application.Internal;
using Application.Models;
using Domain.AggregatesModel;
using Domain.Enumerations;
using Domain.Exceptions;
using MediatR;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, UserDto>
    {
        private readonly IUserRepository _repository;

        public SignInCommandHandler(IUserRepository repository)
            => _repository = repository.NotNull(nameof(repository));

        public async Task<UserDto> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetAsync(request.UserName);

            if (user == null || user.Password != ((PasswordString)request.Password).AsSha256())
            {
                throw new UserDomainException(UserErrorCodes.SignInError, "Username not found or password error");
            }

            if (user.Status != UserStatus.Active)
            {
                throw new UserDomainException(UserErrorCodes.StatusError, $"user is {user.Status}");
            }

            return user.AsDto();
        }
    }
}
