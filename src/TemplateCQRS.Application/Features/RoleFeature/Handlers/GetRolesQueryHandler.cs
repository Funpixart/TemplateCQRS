using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using TemplateCQRS.Application.Features.RoleFeature.Queries;
using TemplateCQRS.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Payload<IQueryable<RoleDto>, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetRolesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Payload<IQueryable<RoleDto>, List<ValidationFailure>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        try
        {
            var result = _unitOfWork.ReadAll<Role>();
            if (!result.Any())
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }

            var roleMapped = _mapper.Map<IQueryable<RoleDto>>(result);

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : Payload<IQueryable<RoleDto>, List<ValidationFailure>>.Success(roleMapped!);
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetRolesQueryHandler>(ex);
        }
    }
}