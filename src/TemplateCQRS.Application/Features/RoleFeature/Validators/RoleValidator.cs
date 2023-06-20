using TemplateCQRS.Application.Features.RoleFeature.Commands;
using FluentValidation;
using TemplateCQRS.Application.Features.RoleFeature.Commands;

namespace TemplateCQRS.Application.Features.RoleFeature.Validators;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.CreateRoleDto).SetValidator(new CreateRoleDtoValidator());
    }
}

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.UpdateRoleDto).SetValidator(new UpdateRoleDtoValidator());
    }
}

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
    }
}

public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 50).WithMessage("Name must be between 1 and 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(1, 150).WithMessage("Description must be between 1 and 150 characters.");

        RuleFor(x => x.AccessLevel)
            .NotEmpty().WithMessage("Access Level is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Access Level must be greater than or equal to 1.");
    }
}

public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 50).WithMessage("Name must be between 1 and 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(1, 150).WithMessage("Description must be between 1 and 150 characters.");

        RuleFor(x => x.AccessLevel)
            .NotEmpty().WithMessage("Access Level is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Access Level must be greater than or equal to 1.");
    }
}