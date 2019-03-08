using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public sealed class UserDomainException:Exception
    {
        public UserDomainException(string errorCode,string message):base(message)
        {
            ErrorCode = errorCode.NotNull(nameof(errorCode));
        }
        public string ErrorCode { get; }

        
    }
}
