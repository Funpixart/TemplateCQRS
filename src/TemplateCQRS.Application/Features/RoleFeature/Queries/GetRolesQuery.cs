using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.RoleFeature.Queries;

public record GetRolesQuery : IRequest<Payload<IQueryable<RoleDto>, List<ValidationFailure>>>;