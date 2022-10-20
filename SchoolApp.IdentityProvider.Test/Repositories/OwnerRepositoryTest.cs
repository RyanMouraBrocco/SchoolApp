using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.IdentityProvider.Test.Repositories;

public class OwnerRepositoryTest : BaseMainEntityRepositoryTest<OwnerDto, Owner, SchoolAppIdentityProviderContext, OwnerRepository>
{
    public OwnerRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new Owner()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "Name test",
            DocumentId = "Document",
            Email = "e@e.com",
            Password = "HASH PASSWORD",
            UpdaterId = null,
            UpdateDate = null
        };

        OwnerRepository ownerRepository = new OwnerRepository(_mockContext.Object);

        await InternalInsertTestAsync(ownerRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new Owner()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "Name test",
            DocumentId = "Document",
            Email = "e@e.com",
            Password = "HASH PASSWORD",
            UpdaterId = null,
            UpdateDate = null,
            Id = 1
        };

        OwnerRepository ownerRepository = new OwnerRepository(_mockContext.Object);

        await InternalUpdateTestAsync(ownerRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<OwnerDto>
        {
            new OwnerDto { Name = "Func 1", Id = 1, AccountId = 2 },
        }.AsQueryable();

        OwnerRepository ownerRepository = new OwnerRepository(_mockContext.Object);

        await InternalDeleteTestAsync(ownerRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        OwnerRepository ownerRepository = new OwnerRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(ownerRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<OwnerDto>()
        {
            new OwnerDto() { Id = 1, AccountId = 1, Name = "Owner 1"},
            new OwnerDto() { Id = 2, AccountId = 2, Name = "Owner 2"},
        }.AsQueryable();
        OwnerRepository ownerRepository = new OwnerRepository(_mockContext.Object);

        InternalGetOneByIdTest(ownerRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        OwnerRepository ownerRepository = new OwnerRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(ownerRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<OwnerDto>()
        {
            new OwnerDto() { Id = 1, AccountId = 1, Name = "Owner 1" },
            new OwnerDto() { Id = 2, AccountId = 2, Name = "Owner 2" },
            new OwnerDto() { Id = 3, AccountId = 1, Name = "Owner 3", Deleted = true },
            new OwnerDto() { Id = 4, AccountId = 3, Name = "Owner 4" },
            new OwnerDto() { Id = 5, AccountId = 4, Name = "Owner 5" },
            new OwnerDto() { Id = 6, AccountId = 1, Name = "Owner 6" },
            new OwnerDto() { Id = 7, AccountId = 1, Name = "Owner 7" },
            new OwnerDto() { Id = 8, AccountId = 3, Name = "Owner 8" }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var ownerRepository = new OwnerRepository(_mockContext.Object);

        // Act
        var result = ownerRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 3);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(6, result[1].Id);
        Assert.Equal(7, result[2].Id);
    }
}
