using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace TemplateCQRS.Application.Features.RoleFeature.Commands;


public record DeleteRoleCommand(Guid Id) : IRequest<Payload<Unit, List<ValidationFailure>>>;