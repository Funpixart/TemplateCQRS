using AutoMapper;
using TemplateCQRS.Application.Features.RoleFeature.Commands;
using TemplateCQRS.Application.Features.RoleFeature.Handlers;
using TemplateCQRS.Application.Features.RoleFeature.Validators;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Domain.Dto.Role;
using TemplateCQRS.Domain.Models;
using TemplateCQRS.Infrastructure.Data;
using TemplateCQRS.Infrastructure.Repositories;
using TemplateCQRS.UnitTest.Data;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace TemplateCQRS.UnitTest.Application.FeaturesTest;

public class RoleHandlersTests
{
    private Mock<RoleManager<Role>> RoleManagerMock { get; set; } = null!;
    private Mock<IRepository<RoleClaim>> RoleClaimRepository { get; set; } = null!;
    private Mock<IMapper> MapperMock { get; set; } = null!;

    private void Setup_RoleManager()
    {
        var roles = MockingData.GenerateRoles().AsQueryable();

        // Create a mock of RoleManager with dependencies mocked
        RoleManagerMock = new Mock<RoleManager<Role>>(
            new Mock<IRoleStore<Role>>().Object,
            Array.Empty<IRoleValidator<Role>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<Role>>>().Object);

        // Setup mock to return our roles when the Roles property is accessed
        RoleManagerMock.Setup(r => r.Roles).Returns(roles);

        RoleManagerMock.Setup(x => x.UpdateAsync(It.IsAny<Role>()))
            .ReturnsAsync(IdentityResult.Success);

        RoleManagerMock.Setup(x => x.DeleteAsync(It.IsAny<Role>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    private void Setup_Mapper()
    {
        // Create a mock for IMapper
        MapperMock = new Mock<IMapper>();

        MapperMock.Setup(m => m.Map<Role>(It.IsAny<UpdateRoleDto>()))
            .Returns(It.IsAny<Role>());

        MapperMock.Setup(m => m.Map<InfoRoleDto>(It.IsAny<Role?>()))
            .Returns(It.IsAny<InfoRoleDto>());
    }

    private void Setup_Repositories()
    {
        var roleClaimsList = MockingData.GenerateRoleClaims();

        RoleClaimRepository = new Mock<IRepository<RoleClaim>>();

        RoleClaimRepository.Setup(x => x.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(roleClaimsList);
    }

    public RoleHandlersTests()
    {
        Setup_RoleManager();
        Setup_Mapper();
        Setup_Repositories();
    }

    [Fact]
    public async void GivenValid_CreateCommand_ShouldReturn_SuccessfulResult()
    {
        // Define a CreateRoleDto instance which represents a new role to be created
        var roleDto = new CreateRoleDto()
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
        MapperMock.Setup(m => m.Map<Role>(It.IsAny<CreateRoleDto>()))
            .Returns(Constants.DefaultRoles.User);

        // Arrange: Setup the mock IMapper to return a new DetailRoleDto when Map is called
        MapperMock.Setup(m => m.Map<CreateRoleDto>(It.IsAny<Role>()))
            .Returns(roleDto);

        // Act: Instantiate the command handler and call the Handle method
        var handler = new CreateRoleCommandHandler(MapperMock.Object, validator, RoleManagerMock.Object);
        var result = await handler.Handle(createCommand, CancellationToken.None);

        // Assert: Check that the result is not null and indicates success
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async void GivenValid_UpdateCommand_ShouldReturn_SuccessfulResult()
    {
        var roleId = MockingData.RoleIds[1];

        var roleDto = new UpdateRoleDto
        {
            AccessLevel = 4,
            Description = "This was updated by the tester",
            Name = "Tester"
        };
        
        var updateCommand = new UpdateRoleCommand(roleId, roleDto);
        var validator = new UpdateRoleCommandValidator();
        var validationResult = await validator.ValidateAsync(updateCommand);

        // Assert that the command is valid
        Assert.True(validationResult.IsValid);

        // Act: Instantiate the command handler and call the Handle method
        var handler = new UpdateRoleCommandHandler(MapperMock.Object, validator, RoleManagerMock.Object, RoleClaimRepository.Object);
        var result = await handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        RoleManagerMock.Verify(x => x.UpdateAsync(It.IsAny<Role>()), Times.Once);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async void GivenValid_DeleteCommand_ShouldReturn_SuccessfulResult()
    {
        var roleId = MockingData.RoleIds[1];
        var roleList = MockingData.GenerateRoles();
        var role = roleList.FirstOrDefault(x => x.Id == roleId);
        
        var deleteCommand = new DeleteRoleCommand(role.Id);
        var validator = new DeleteRoleCommandValidator();
        var validationResult = await validator.ValidateAsync(deleteCommand);

        // Assert that the command is valid
        Assert.True(validationResult.IsValid);

        // Act: Instantiate the command handler and call the Handle method
        var handler = new DeleteRoleCommandHandler(validator, RoleManagerMock.Object);
        var result = await handler.Handle(deleteCommand, CancellationToken.None);

        // Assert
        RoleManagerMock.Verify(x => x.DeleteAsync(It.IsAny<Role>()), Times.Once);
        Assert.True(result.IsSuccess);
    }
}