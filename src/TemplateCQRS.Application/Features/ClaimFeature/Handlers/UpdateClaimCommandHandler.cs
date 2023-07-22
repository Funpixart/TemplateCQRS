using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using TemplateCQRS.Application.Features.ClaimFeature.Commands;
using TemplateCQRS.Application.Features.ClaimFeature.Validators;
using TemplateCQRS.Application.Features.RoleFeature.Handlers;
using TemplateCQRS.Domain.Dto.Claim;
using TemplateCQRS.Infrastructure.Data;

namespace TemplateCQRS.Application.Features.ClaimFeature.Handlers;

public class UpdateClaimCommandHandler : IRequestHandler<UpdateClaimCommand, Payload<InfoClaimDto, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UpdateClaimCommandValidator _validator;
    private readonly IRepository<RoleClaim> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClaimCommandHandler(IMapper mapper, UpdateClaimCommandValidator validator, IRepository<RoleClaim> repo, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _validator = validator;
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Payload<InfoClaimDto, List<ValidationFailure>>> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
    {
        // Catch possible exceptions and let the others exception propagate.
        try
        {
            // Validate the request.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If there were any validation errors, return a failure payload.
            if (validationResult.Errors.Count > 0) return validationResult.Errors;

            // Get the model by the Id
            var claimFound = await _unitOfWork.FindByKey<RoleClaim>(request.Id);

            if (claimFound is not null)
            {
                // Map the dto to the model.
                _mapper.Map(request.UpdateClaimDto, claimFound);

                _ = await _repo.UpdateAsync(claimFound);
            }
            else
            {
                validationResult.Errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status400BadRequest.ToString(),
                    ErrorMessage = "claim not found with the id provided."
                });
            }

            // If there were any validation errors, return a failure payload.
            return validationResult.Errors.Count > 0 ? validationResult.Errors : _mapper.Map<InfoClaimDto>(claimFound);
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<UpdateRoleCommandHandler>(ex);
        }
    }
}
