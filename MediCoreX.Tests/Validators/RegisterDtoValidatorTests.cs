using FluentValidation.TestHelper;
using MediCoreX.Api.DTOs;
using MediCoreX.Api.Validators;

namespace MediCoreX.Tests.Validators
{
    public class RegisterDtoValidatorTests
    {
        private readonly RegisterDtoValidator _validator;

        public RegisterDtoValidatorTests()
        {
            _validator = new RegisterDtoValidator();
        }

        [Fact]
        public void ValidRegisterDto_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FullName = "Rahul Sharma",
                Email = "rahul@gmail.com",
                Password = "Password123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
         
         [Fact]
public void EmptyFullName_ShouldHaveValidationError()
{
    // Arrange
    var dto = new RegisterDto
    {
        FullName = "",
        Email = "rahul@gmail.com",
        Password = "Password123"
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.FullName)
        .WithErrorMessage("Full name is required.");
}

[Fact]
public void FullName_Exceeds100Characters_ShouldHaveValidationError()
{
    // Arrange
    var dto = new RegisterDto
    {
        FullName = new string('A', 101),
        Email = "rahul@gmail.com",
        Password = "Password123"
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.FullName)
        .WithErrorMessage("Full name cannot exceed 100 characters.");
}

[Fact]
public void EmptyEmail_ShouldHaveValidationError()
{
    // Arrange
    var dto = new RegisterDto
    {
        FullName = "Rahul Sharma",
        Email = "",
        Password = "Password123"
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Email)
        .WithErrorMessage("Email is required.");
}

[Fact]
public void InvalidEmail_ShouldHaveValidationError()
{
    // Arrange
    var dto = new RegisterDto
    {
        FullName = "Rahul Sharma",
        Email = "invalid-email",
        Password = "Password123"
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Email)
        .WithErrorMessage("Please provide a valid email address.");
}
[Fact]
public void EmptyPassword_ShouldHaveValidationError()
{
    // Arrange
    var dto = new RegisterDto
    {
        FullName = "Rahul Sharma",
        Email = "rahul@gmail.com",
        Password = ""
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Password)
        .WithErrorMessage("Password is required.");
}

[Fact]
public void PasswordLessThan8Characters_ShouldHaveValidationError()
{
    // Arrange
    var dto = new RegisterDto
    {
        FullName = "Rahul Sharma",
        Email = "rahul@gmail.com",
        Password = "Pass123"
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Password)
        .WithErrorMessage("Password must be at least 8 characters.");
}


    }

}