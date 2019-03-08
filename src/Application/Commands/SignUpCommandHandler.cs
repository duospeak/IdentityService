using Application.Models;
using Domain.AggregatesModel;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, UserDto>
    {
        private readonly IUserRepository _repository;

        public SignUpCommandHandler(IUserRepository repository)
            => _repository = repository.NotNull(nameof(repository));

        public async Task<UserDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _repository.GetAsync(request.UserName, cancellationToken);

            if (existingUser != null)
            {
                throw new UserDomainException(UserErrorCodes.SignedUp, "username has already signed up by others");
            }

            existingUser = new ApplicationUser(request.UserName, request.Password);

            await _repository.AddAsync(existingUser);

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return new UserDto()
            {
                Status = existingUser.Status
            };
        }
    }
}
