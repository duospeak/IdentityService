using Application.Commands;
using Application.Validators;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests.ValidatorTests
{
    public class SignUpValidatorTests
    {
        public SignUpCommandValidator CreateValidator()
        {
            return new SignUpCommandValidator();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        public void Should_have_error(string username, string password)
        {
            var command = new SignUpCommand()
            {
                UserName = username,
                Password = password
            };

            var validator = CreateValidator();

            validator.ShouldHaveValidationErrorFor(x => x.UserName, command);
            validator.ShouldHaveValidationErrorFor(x => x.Password, command);
        }

        [Theory]
        [InlineData("0123456", "0123456")]
        [InlineData("012345678901234567890", "012345678901234567890")]
        public void Should_not_have_error(string username, string password)
        {
            var command = new SignUpCommand()
            {
                UserName = username,
                Password = password
            };

            var validator = CreateValidator();

            validator.ShouldNotHaveValidationErrorFor(x => x.UserName, command);
            validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
        }
    }
}
