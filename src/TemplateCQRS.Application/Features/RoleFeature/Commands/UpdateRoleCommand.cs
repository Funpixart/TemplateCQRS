using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateCQRS.Application.Features.RoleFeature.Commands;

public record UpdateRoleCommand(Guid Id, RoleDto UpdateRoleDto) : IRequest<Payload<RoleDto, List<ValidationFailure>>>;