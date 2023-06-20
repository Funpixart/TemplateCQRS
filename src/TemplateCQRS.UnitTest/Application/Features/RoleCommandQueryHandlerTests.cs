using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Handlers;
using TemplateCQRS.Application.Features.RoleFeature.Validators;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Domain.Dto.Role;
using TemplateCQRS.Domain.Models;
using TemplateCQRS.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TemplateCQRS.UnitTest.Application.Features;

public class RoleCommandQueryHandlerTests
{
    private Mock<RoleManager<Role>> RoleManagerMock { get; set; } = null!;
    private Mock<IMapper> MapperMock { get; set; } = null!;

    private void Setup_RoleManager()
    {
        // Create a list of roles (currently with one role) and convert it to Queryable
        var roles = new List<Role>
        {
            Constants.DefaultRoles.User,
            Constants.DefaultRoles.Visitor,
            Constants.DefaultRoles.Admin,
            Constants.DefaultRoles.Author,
            Constants.DefaultRoles.Contributor,
            Constants.DefaultRoles.Manager,
            Constants.DefaultRoles.Supervisor,
            Constants.DefaultRoles.Owner,
        }.AsQueryable();

        // Create a mock of RoleManager with dependencies mocked
        RoleManagerMock = new Mock<RoleManager<Role>>(
            new Mock<IRoleStore<Role>>().Object,
            Array.Empty<IRoleValidator<Role>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<Role>>>().Object);

        // Setup mock to return our roles when the Roles property is accessed
        RoleManagerMock.Setup(r => r.Roles).Returns(roles);
    }

    private void Setup_Mapper()
    {
        // Create a mock for IMapper
        MapperMock = new Mock<IMapper>();
    }
}