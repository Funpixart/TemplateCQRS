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

    [Fact]
    public async void Handle_GivenValidCreateCommand_ShouldReturnSuccessfulResult()
    {
        Setup_RoleManager();
        Setup_Mapper();

        // Define a CreateRoleDto instance which represents a new role to be created
        var roleDto = new RoleDto()
        {
            AccessLevel = 1,
            Description = "Unit Test Role 2",
            Name = "Tester 2"
        };

        // Create a new CreateRoleCommand instance using the DTO for the new role
        var createCommand = new CreateRoleCommand(roleDto);

        // Create an instance of the CreateRoleCommandValidator
        var validator = new CreateRoleCommandValidator();

        // Validate the command
        var validationResult = await validator.ValidateAsync(createCommand);

        // Assert that the command is valid
        Assert.True(validationResult.IsValid);

        // Arrange: Setup the mock RoleManager to return a success result when CreateAsync is called
        RoleManagerMock.Setup(x => x.CreateAsync(It.IsAny<Role>()))
            .ReturnsAsync(IdentityResult.Success);

        // Arrange: Setup the mock IMapper to return a new Role when Map is called
        MapperMock.Setup(m => m.Map<Role>(It.IsAny<RoleDto>()))
            .Returns(Constants.DefaultRoles.User);

        // Arrange: Setup the mock IMapper to return a new DetailRoleDto when Map is called
        MapperMock.Setup(m => m.Map<RoleDto>(It.IsAny<Role>()))
            .Returns(roleDto);

        // Act: Instantiate the command handler and call the Handle method
        var handler = new CreateRoleCommandHandler(MapperMock.Object, validator, RoleManagerMock.Object);
        var result = await handler.Handle(createCommand, CancellationToken.None);

        // Assert: Check that the result is not null and indicates success
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async void Handle_GivenValidUpdateCommand_ShouldReturnSuccessfulResult()
    {
        Setup_RoleManager();
        Setup_Mapper();
        
        var roleDto = new RoleDto()
        {
            AccessLevel = 4,
            Description = "Updated Test",
            Name = "Tester Updated"
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var updateCommand = new UpdateRoleCommand(Constants.DefaultRoles.User.Id, roleDto);
        var validator = new UpdateRoleCommandValidator();
        var validationResult = await validator.ValidateAsync(updateCommand);

        // Assert that the command is valid
        Assert.True(validationResult.IsValid);

        // Arrange: Setup the mock RoleManager to return a success result when UpdateAsync is called
        RoleManagerMock.Setup(x => x.UpdateAsync(It.IsAny<Role>()))
            .ReturnsAsync(IdentityResult.Success);

        // Arrange: Setup the mock IMapper to return a Default Role when Map is called
        MapperMock.Setup(m => m.Map<Role>(It.IsAny<RoleDto>()))
            .Returns(Constants.DefaultRoles.User);

        // Arrange: Setup the mock IMapper to return a new RoleDto when Map is called
        MapperMock.Setup(m => m.Map<RoleDto>(It.IsAny<Role>()))
            .Returns(roleDto);

        // Arrange: Setup the mock UnitOfWork to return the store role when FindByKey is called
        unitOfWorkMock.Setup(x => x.FindByKey<Role>(It.IsAny<Guid>()))
            .ReturnsAsync(Constants.DefaultRoles.User);

        // Act: Instantiate the command handler and call the Handle method
        var handler = new UpdateRoleCommandHandler(MapperMock.Object, validator, RoleManagerMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        RoleManagerMock.Verify(x => x.UpdateAsync(It.Is<Role>(r => r.Id == Constants.DefaultRoles.User.Id)), Times.Once);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async void Handle_GivenValidDeleteCommand_ShouldReturnSuccessfulResult()
    {
        Setup_RoleManager();
        Setup_Mapper();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var deleteCommand = new DeleteRoleCommand(Constants.DefaultRoles.User.Id);
        var validator = new DeleteRoleCommandValidator();
        var validationResult = await validator.ValidateAsync(deleteCommand);

        // Assert that the command is valid
        Assert.True(validationResult.IsValid);

        // Arrange: Setup the mock UnitOfWork to return the store role when FindByKey is called
        unitOfWorkMock.Setup(x => x.FindByKey<Role>(Constants.DefaultRoles.User.Id))
            .ReturnsAsync(Constants.DefaultRoles.User);

        // Arrange: Setup the mock RoleManager to return a success result when UpdateAsync is called
        RoleManagerMock.Setup(x => x.DeleteAsync(It.Is<Role>(x => Constants.DefaultRoles.User.Id  == x.Id)))
            .ReturnsAsync(IdentityResult.Success);

        // Act: Instantiate the command handler and call the Handle method
        var handler = new DeleteRoleCommandHandler(MapperMock.Object, validator, unitOfWorkMock.Object, RoleManagerMock.Object);
        var result = await handler.Handle(deleteCommand, CancellationToken.None);

        // Assert
        RoleManagerMock.Verify(x => x.DeleteAsync(It.Is<Role>(r => r.Id == Constants.DefaultRoles.User.Id)), Times.Once);
        Assert.True(result.IsSuccess);
    }
}