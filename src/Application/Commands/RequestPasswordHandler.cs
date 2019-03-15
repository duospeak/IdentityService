using Application.Internal;
using Domain.AggregatesModel;
using Domain.Enumerations;
using Domain.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class RequestPasswordHandler : IRequestHandler<RequestPasswordCommand>
    {
        private readonly IUserRepository _repository;

        public RequestPasswordHandler(IUserRepository repository) 
            => _repository = repository.NotNull(nameof(repository));

        public async Task<Unit> Handle(RequestPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetAsync(request.UserName, cancellationToken);

            if (user.IsNull())
            {
                throw new UserDomainException(UserErrorCodes.UserIsNull, "no user found");
            }

            if (user.Status != UserStatus.Active)
            {
                throw new UserDomainException(UserErrorCodes.StatusError, $"user is {user.Status}");
            }

            if (user.SecretCode != request.SecretCode)
            {
                throw new UserDomainException(UserErrorCodes.SecretCodeError, "secret is invalid");
            }

            user.ClearSecretCode();

            user.SetNewPassword(((PasswordString)request.Password).AsSha256());

            _repository.Update(user);

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
