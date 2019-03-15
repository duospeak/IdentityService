using Domain.AggregatesModel;
using Domain.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class SendRequestPasswordSecretCodeHandler : IRequestHandler<SendRequestPasswordSecretCodeCommand>
    {
        private readonly IUserRepository _repository;

        public SendRequestPasswordSecretCodeHandler(IUserRepository repository)
            => _repository = repository.NotNull(nameof(repository));

        public async Task<Unit> Handle(SendRequestPasswordSecretCodeCommand request, CancellationToken cancellationToken)
        {
            request.NotNull(nameof(request));

            var user = await _repository.GetAsync(request.UserName);

            if (user.IsNull())
            {
                throw new UserDomainException(UserErrorCodes.UserIsNull, "user not exist!");
            }

            if (request.Accept == SendRequestPasswordSecretCodeCommand.AcceptType.Sms
                && user.PhoneNumber.IsNull())
            {
                throw new UserDomainException(UserErrorCodes.PhoneIsNull, "phone not set");
            }

            if (user.SecretCode.IsNull())
            {
                var code = new Random().Next(999999).ToString();

                user.SetSecretCode(code);
            }
            // send user a sms with verify code

            return Unit.Value;
        }
    }
}
