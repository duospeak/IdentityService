using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public static class UserErrorCodes
    {
        public const string StatusError = "USER_STATUS_ERROR";
        public const string SignedUp = "USER_SIGNED_UP";
        public const string SignInError = "SIGN_IN_ERROR";
        public const string UserIsNull = "USER_NOT_EXIST";
        public const string SecretCodeError = "SECRET_CODE_ERROR";
        public const string PhoneIsNull = "PHONE_NOT_SET";
    }
}
