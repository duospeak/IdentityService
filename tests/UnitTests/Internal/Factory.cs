using Application.Commands;
using Application.Models;
using Domain.AggregatesModel;
using Domain.Enumerations;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Internal
{
    static class Factory
    {
        private const string TestUserName = "test-user";
        private const string TestPassword = "test-password";
        private const long TestUserId = 1;
        private const UserStatus TestUserStatus = UserStatus.Active;

        public static UserDto CreateTestUserDto(long id = TestUserId, string userName = TestUserName, UserStatus userStatus = TestUserStatus)
        {
            return new UserDto()
            {
                Id = id,
                UserName = userName,
                Status = userStatus
            };
        }

        public static SignInCommand CreateTestSignInCommand(string userName = TestUserName, string password = TestPassword, bool rememberMe = true)
        {
            return new SignInCommand
            {
                UserName = userName,
                Password = password,
                RememberMe = rememberMe
            };
        }

        public static ApplicationUser CreateTestApplicationUser(string userName = TestUserName, string password = TestPassword, UserStatus status = TestUserStatus)
        {
            var user = new ApplicationUser(userName, password.Sha256());

            switch (status)
            {
                case UserStatus.Active:
                    user.Active();
                    break;
                case UserStatus.AwaitingActive:
                    break;
                case UserStatus.Blocked:
                    user.Active();
                    user.Block();
                    break;
                default:
                    break;
            }

            user.ClearDomainEvents();

            return user;
        }
    }
}
