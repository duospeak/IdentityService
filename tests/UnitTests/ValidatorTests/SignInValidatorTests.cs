using Application.Commands;
using Application.Validators;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ValidatorTests
{
    public class SignInValidatorTests
    {
        private SignInCommandValidator CreateValidator()
        {
            return new SignInCommandValidator();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        [InlineData("12345", "12345")]
        [InlineData("012345678901234567890", "012345678901234567890")]
        public void Should_have_error(string password, string username)
        {
            var validator = CreateValidator();

            var command = new SignInCommand()
            {
                Password = password,
                RememberMe = false,
                UserName = username
            };

            validator.ShouldHaveValidationErrorFor(c => c.Password, command);
            validator.ShouldHaveValidationErrorFor(c => c.UserName, command);
        }


        [Theory]
        [InlineData("012345", "012345")]
        [InlineData("01234567890123456789", "01234567890123456789")]
        public void Should_not_have_error(string username, string password)
        {
            var command = new SignInCommand()
            {
                Password = password,
                RememberMe = false,
                UserName = username
            };
            var validator = CreateValidator();

            validator.ShouldNotHaveValidationErrorFor(x => x.UserName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
        }
    }
}
