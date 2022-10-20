using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.IdentityProvider.Test.Repositories;

public class MessageAllowedClassroomRepositoryTest : BaseRepositoryTest<IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedClassroomDto, IdentityProvider.Application.Domain.Dtos.MessageAllowedClassroomDto, SchoolAppIdentityProviderContext, MessageAllowedClassroomRepository>
{
    public MessageAllowedClassroomRepositoryTest() : base()
    {

    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new IdentityProvider.Application.Domain.Dtos.MessageAllowedClassroomDto()
        {
            MessageId = "message id",
            ClassroomId = 1
        };

        MessageAllowedClassroomRepository messageAllowedClassroomRepository = new MessageAllowedClassroomRepository(_mockContext.Object);

        await InternalInsertTestAsync(messageAllowedClassroomRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new IdentityProvider.Application.Domain.Dtos.MessageAllowedClassroomDto()
        {
            MessageId = "message id",
            ClassroomId = 1
        };

        MessageAllowedClassroomRepository messageAllowedClassroomRepository = new MessageAllowedClassroomRepository(_mockContext.Object);

        await InternalUpdateTestAsync(messageAllowedClassroomRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedClassroomDto>
        {
            new IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedClassroomDto { Id = 1, MessageId = "message id", ClassroomId = 1 },
        }.AsQueryable();

        MessageAllowedClassroomRepository messageAllowedClassroomRepository = new MessageAllowedClassroomRepository(_mockContext.Object);

        await InternalDeleteTestAsync(messageAllowedClassroomRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        MessageAllowedClassroomRepository messageAllowedClassroomRepository = new MessageAllowedClassroomRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(messageAllowedClassroomRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedClassroomDto>()
        {
            new IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedClassroomDto() { Id = 1, MessageId = "message id", ClassroomId = 1},
            new IdentityProvider.Sql.Dtos.MessageAllowedPermissions.MessageAllowedClassroomDto() { Id = 2, MessageId = "message id 2", ClassroomId = 2},
        }.AsQueryable();
        MessageAllowedClassroomRepository messageAllowedClassroomRepository = new MessageAllowedClassroomRepository(_mockContext.Object);

        InternalGetOneByIdTest(messageAllowedClassroomRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        MessageAllowedClassroomRepository messageAllowedClassroomRepository = new MessageAllowedClassroomRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(messageAllowedClassroomRepository, 3);
    }
}
