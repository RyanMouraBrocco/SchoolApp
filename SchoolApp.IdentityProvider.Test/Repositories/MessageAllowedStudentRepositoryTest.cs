using SchoolApp.IdentityProvider.Application.Domain.Dtos;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Repositories;
using SchoolApp.IdentityProvider.Test.Repositories.Base;

namespace SchoolApp.IdentityProvider.Test.Repositories;

public class MessageAllowedStudentRepositoryTest : BaseRepositoryTest<IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedStudentDto, IdentityProvider.Application.Domain.Dtos.MessageAllowedStudentDto, SchoolAppIdentityProviderContext, MessageAllowedStudentRepository>
{
    public MessageAllowedStudentRepositoryTest() : base()
    {

    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new IdentityProvider.Application.Domain.Dtos.MessageAllowedStudentDto()
        {
            MessageId = "message id",
            StudentId = 1
        };

        MessageAllowedStudentRepository messageAllowedStudentRepository = new MessageAllowedStudentRepository(_mockContext.Object);

        await InternalInsertTestAsync(messageAllowedStudentRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new IdentityProvider.Application.Domain.Dtos.MessageAllowedStudentDto()
        {
            MessageId = "message id",
            StudentId = 1
        };

        MessageAllowedStudentRepository messageAllowedStudentRepository = new MessageAllowedStudentRepository(_mockContext.Object);

        await InternalUpdateTestAsync(messageAllowedStudentRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedStudentDto>
        {
            new IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedStudentDto { Id = 1, MessageId = "message id", StudentId = 1 },
        }.AsQueryable();

        MessageAllowedStudentRepository messageAllowedStudentRepository = new MessageAllowedStudentRepository(_mockContext.Object);

        await InternalDeleteTestAsync(messageAllowedStudentRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        MessageAllowedStudentRepository messageAllowedStudentRepository = new MessageAllowedStudentRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(messageAllowedStudentRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedStudentDto>()
        {
            new IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedStudentDto() { Id = 1, MessageId = "message id", StudentId = 1},
            new IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedStudentDto() { Id = 2, MessageId = "message id 2", StudentId = 2},
        }.AsQueryable();
        MessageAllowedStudentRepository messageAllowedStudentRepository = new MessageAllowedStudentRepository(_mockContext.Object);

        InternalGetOneByIdTest(messageAllowedStudentRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        MessageAllowedStudentRepository messageAllowedStudentRepository = new MessageAllowedStudentRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(messageAllowedStudentRepository, 3);
    }
}
