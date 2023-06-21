using FluentValidation;
using TemplateCQRS.Application.Features.ClaimFeature.Commands;
using TemplateCQRS.Domain.Dto.Claim;

namespace TemplateCQRS.Application.Features.ClaimFeature.Validators;

public class CreateClaimCommandValidator : AbstractValidator<CreateClaimCommand>
{
    public CreateClaimCommandValidator()
    {
        RuleFor(x => x.CreateClaimDto).SetValidator(new CreateClaimDtoValidator());
    }
}

public class UpdateClaimCommandValidator : AbstractValidator<UpdateClaimCommand>
{
    public UpdateClaimCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(0);
        RuleFor(x => x.UpdateClaimDto).SetValidator(new UpdateClaimDtoValidator());
    }
}

public class DeleteClaimCommandValidator : AbstractValidator<DeleteClaimCommand>
{
    public DeleteClaimCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(0);
    }
}

public class CreateClaimDtoValidator : AbstractValidator<CreateClaimDto>
{
    public CreateClaimDtoValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required.");

        RuleFor(x => x.ClaimType)
            .NotEmpty().WithMessage("Claim Type is required.")
            .Length(1, 50).WithMessage("Claim Type must be between 1 and 50 characters.");

        RuleFor(x => x.ClaimValue)
            .NotEmpty().WithMessage("ClaimValue is required.");
    }
}

public class UpdateClaimDtoValidator : AbstractValidator<UpdateClaimDto>
{
    public UpdateClaimDtoValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required.");

        RuleFor(x => x.ClaimType)
            .NotEmpty().WithMessage("Claim Type is required.")
            .Length(1, 50).WithMessage("Claim Type must be between 1 and 50 characters.");

        RuleFor(x => x.ClaimValue)
            .NotEmpty().WithMessage("ClaimValue is required.");
    }
}
