using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TemplateCQRS.Application.Features.UserFeature.Queries;

namespace TemplateCQRS.Application.Features.RoleFeature.Handlers;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Payload<List<InfoUserDto>, List<ValidationFailure>>>
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetAllUserQueryHandler(IMapper mapper, UserManager<User> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Payload<List<InfoUserDto>, List<ValidationFailure>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<ValidationFailure>();
        try
        {
            var result = _userManager.Users.ToList();
            if (!result.Any())
            {
                errors.Add(new ValidationFailure
                {
                    ErrorCode = StatusCodes.Status204NoContent.ToString(),
                    ErrorMessage = "Nothing to return."
                });
                return errors;
            }

            var mappedUsers = _mapper.Map<List<InfoUserDto>>(result.ToList());

            // If there were any validation errors, return a failure payload.
            return errors.Count > 0 ? errors : mappedUsers;
        }
        catch (Exception ex)
        {
            return ErrorHelper.LogExceptionAndReturnError<GetAllRoleQueryHandler>(ex);
        }
    }
}