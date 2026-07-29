using FluentValidation;
using MediCoreX.Api.DTOs;

namespace MediCoreX.Api.Validators;

public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
{
    private static readonly string[] AllowedGenders = ["Male", "Female", "Other"];

    public UpdatePatientDtoValidator()
    {
        RuleFor(patient => patient.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

        RuleFor(patient => patient.Age)
            .InclusiveBetween(0, 130)
            .WithMessage("Age must be between 0 and 130.");

        RuleFor(patient => patient.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(gender => AllowedGenders.Contains(gender, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Gender must be Male, Female, or Other.");
    }
}
