using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.ClaimFeature.Commands;
using TemplateCQRS.Application.Features.ClaimFeature.Validators;
using TemplateCQRS.Domain.Dto.Claim;

namespace TemplateCQRS.Application.Features.ClaimFeature.Handlers;

public class CreateClaimCommandHandler : IRequestHandler<CreateClaimCommand, Payload<InfoClaimDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly CreateClaimCommandValidator _validator;
    private readonly IRepository<RoleClaim> _claimRepository;
    private readonly RoleManager<Role> _roleRepository;

    public CreateClaimCommandHandler(IMapper mapper, CreateClaimCommandValidator validator, IRepository<RoleClaim> claimRepository, RoleManager<Role> roleRepository)
    {
        _mapper = mapper;
        _validator = validator;
        _claimRepository = claimRepository;
        _roleRepository = roleRepository;
    }
    public async Task<Payload<InfoClaimDto, List<ValidationFailure>>> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // Map the dto to the model.
            var claim = _mapper.Map<RoleClaim>(request.CreateClaimDto);

            var role = await _roleRepository.FindByIdAsync(request.CreateClaimDto.RoleId.ToString());

            if (role is null)
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "Role ID not found with the id provided."
                });
            }

            // Create the model.
            var result = await _claimRepository.CreateAsync(claim);


            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            return _mapper.Map<InfoClaimDto>(claim);
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<CreateClaimCommandHandler>(ex);
        }
    }
}
