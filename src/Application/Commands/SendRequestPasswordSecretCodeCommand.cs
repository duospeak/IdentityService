using MediatR;

namespace Application.Commands
{
    public class SendRequestPasswordSecretCodeCommand : IRequest
    {
        public string UserName { get; set; }

        public AcceptType Accept { get; set; }
        public enum AcceptType
        {
            Sms,
        }
    }
    
}
