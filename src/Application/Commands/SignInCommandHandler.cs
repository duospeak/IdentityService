using Application.Models;
using Domain.AggregatesModel;
using Domain.Enumerations;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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

            var requestPassword = Sha256(request.Password);

            if (user == null || user.Password != requestPassword)
            {
                throw new UserDomainException(UserErrorCodes.SignInError, "Username not found or password error");
            }

            if (user.Status == UserStatus.Blocked)
            {
                throw new UserDomainException(UserErrorCodes.StatusError, "user un actived");
            }

            return user.AsDto();
        }

        private string Sha256(string input)
        {
            using (var sHA = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(sHA.ComputeHash(bytes));
            }
        }
    }
}
