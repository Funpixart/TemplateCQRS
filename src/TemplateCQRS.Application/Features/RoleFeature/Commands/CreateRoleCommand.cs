using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateCQRS.Domain.Dto.Role;

namespace TemplateCQRS.Application.Features.RoleFeature.Commands;

public record CreateRoleCommand(RoleDto CreateRoleDto) : IRequest<Payload<RoleDto, List<ValidationFailure>>>;
