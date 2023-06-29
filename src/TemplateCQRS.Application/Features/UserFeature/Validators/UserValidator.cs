using FluentValidation;
using TemplateCQRS.Application.Features.UserFeature.Commands;
using TemplateCQRS.Application.Features.UserFeature.Queries;

namespace TemplateCQRS.Application.Features.UserFeature.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.CreateUserDto).SetValidator(new CreateUserDtoValidator());
    }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.UpdateUserDto).SetValidator(new UpdateUserDtoValidator());
    }
}

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
    }
}

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .Length(4, 75).WithMessage("Username must be between 4 and 75 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(?:[0-9]+\s?)*$")
            .WithMessage("Phone number is not valid.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .Length(4, 75).WithMessage("Username must be between 4 and 75 characters.")
            .When(x => !string.IsNullOrEmpty(x.UserName));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}

public class GetUserByQueryValidator : AbstractValidator<GetUserByQuery>
{
    public GetUserByQueryValidator()
    {
        RuleFor(x => x.GetUserDto).SetValidator(new GetUserDtoValidator());
    }
}

public class GetUserDtoValidator : AbstractValidator<GetUserDto>
{
    public GetUserDtoValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty)
            .When(x => x.Id is not null);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email is not valid.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.UserName)
            .Length(4, 75).WithMessage("Username must be between 4 and 75 characters.")
            .When(x => !string.IsNullOrEmpty(x.UserName));
    }
}