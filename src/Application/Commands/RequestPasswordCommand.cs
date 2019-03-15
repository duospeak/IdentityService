using MediatR;

namespace Application.Commands
{
    /// <summary>
    /// Forget password,request a new one
    /// </summary>
    public class RequestPasswordCommand : IRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string SecretCode { get; set; }
    }
}
