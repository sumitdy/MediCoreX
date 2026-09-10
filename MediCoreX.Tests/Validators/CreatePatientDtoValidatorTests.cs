using FluentValidation.TestHelper;
using MediCoreX.Api.DTOs;
using MediCoreX.Api.Validators;

namespace MediCoreX.Tests.Validators
{
    public class CreatePatientDtoValidatorTests
    {
        private readonly CreatePatientDtoValidator _validator;

        public CreatePatientDtoValidatorTests()
        {
            _validator = new CreatePatientDtoValidator();
        }

        [Fact]
        public void ValidCreatePatientDto_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "Rahul Sharma",
                Age = 30,
                Gender = "Male"
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
    var dto = new CreatePatientDto
    {
        FullName = "",
        Age = 30,
        Gender = "Male"
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
    var dto = new CreatePatientDto
    {
        FullName = new string('A', 101),
        Age = 30,
        Gender = "Male"
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.FullName)
        .WithErrorMessage("Full name cannot exceed 100 characters.");
}

[Fact]
public void AgeAbove130_ShouldHaveValidationError()
{
    // Arrange
    var dto = new CreatePatientDto
    {
        FullName = "Rahul Sharma",
        Age = 131,
        Gender = "Male"
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Age)
        .WithErrorMessage("Age must be between 0 and 130.");
}

[Fact]
public void EmptyGender_ShouldHaveValidationError()
{
    // Arrange
    var dto = new CreatePatientDto
    {
        FullName = "Rahul Sharma",
        Age = 30,
        Gender = ""
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Gender)
        .WithErrorMessage("Gender is required.");
}

[Fact]
public void InvalidGender_ShouldHaveValidationError()
{
    // Arrange
    var dto = new CreatePatientDto
    {
        FullName = "Rahul Sharma",
        Age = 30,
        Gender = "Unknown"
    };

    // Act
    var result = _validator.TestValidate(dto);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Gender)
        .WithErrorMessage("Gender must be Male, Female, or Other.");
}



    }
}