using FluentValidation;
using TemplateCQRS.Application.Features.UserFeature.Commands;
using TemplateCQRS.Domain.Dto.User;

namespace TemplateCQRS.Application.Features.UserFeature.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.CreateUserDto).SetValidator(new CreateUserDtoValidator());
    }
}

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.")
            .Length(4, 30).WithMessage("Username must be between 4 and 30 characters.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(?:[0-9]+\s?)*$")
            .WithMessage("Phone number is not valid.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}