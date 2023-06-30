using TemplateCQRS.Domain.Models;
using TemplateCQRS.UnitTest.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateCQRS.UnitTest.Application.FeaturesTest;

public class UserHandlersTests
{
    private Mock<UserManager<User>> UserManagerMock { get; set; } = null!;

    private void Setup_UserManager()
    {
        var users = MockingData.GenerateUsers().AsQueryable();

        // Create a mock of UserManager with dependencies mocked
        UserManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            Array.Empty<UserValidator<User>>(),
            Array.Empty<PasswordValidator<User>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);

        // Setup mock to return our users when the Users property is accessed
        UserManagerMock.Setup(u => u.Users).Returns(users);
    }

    public UserHandlersTests()
    {
        Setup_UserManager();
    }
}