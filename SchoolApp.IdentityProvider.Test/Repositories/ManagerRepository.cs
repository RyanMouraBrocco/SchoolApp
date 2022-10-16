using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Repositories;
using SchoolApp.IdentityProvider.Test.Repositories.Base;

namespace SchoolApp.IdentityProvider.Test.Repositories;

public class ManagerRepositoryTest : BaseMainEntityRepositoryTest<ManagerDto, Manager, SchoolAppIdentityProviderContext, ManagerRepository>
{
    public ManagerRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new Manager()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "Name test",
            DocumentId = "Document",
            Email = "e@e.com",
            FunctionId = 1,
            Password = "HASH PASSWORD",
            HiringDate = DateTime.Now,
            Salary = 200,
            UpdaterId = null,
            UpdateDate = null
        };

        ManagerRepository managerRepository = new ManagerRepository(_mockContext.Object);

        await InternalInsertTestAsync(managerRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new Manager()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "Name test",
            DocumentId = "Document",
            Email = "e@e.com",
            FunctionId = 1,
            Password = "HASH PASSWORD",
            HiringDate = DateTime.Now,
            Salary = 200,
            UpdaterId = null,
            UpdateDate = null,
            Id = 1
        };

        ManagerRepository managerRepository = new ManagerRepository(_mockContext.Object);

        await InternalUpdateTestAsync(managerRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<ManagerDto>
        {
            new ManagerDto { Name = "Func 1", Id = 1, AccountId = 2 },
        }.AsQueryable();

        ManagerRepository managerRepository = new ManagerRepository(_mockContext.Object);

        await InternalDeleteTestAsync(managerRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        ManagerRepository managerRepository = new ManagerRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(managerRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<ManagerDto>()
        {
            new ManagerDto() { Id = 1, AccountId = 1, Name = "Manager 1"},
            new ManagerDto() { Id = 2, AccountId = 2, Name = "Manager 2"},
        }.AsQueryable();
        ManagerRepository managerRepository = new ManagerRepository(_mockContext.Object);

        InternalGetOneByIdTest(managerRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        ManagerRepository managerRepository = new ManagerRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(managerRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<ManagerDto>()
        {
            new ManagerDto() { Id = 1, AccountId = 1, Name = "Manager 1" },
            new ManagerDto() { Id = 2, AccountId = 2, Name = "Manager 2" },
            new ManagerDto() { Id = 3, AccountId = 1, Name = "Manager 3", Deleted = true },
            new ManagerDto() { Id = 4, AccountId = 3, Name = "Manager 4" },
            new ManagerDto() { Id = 5, AccountId = 4, Name = "Manager 5" },
            new ManagerDto() { Id = 6, AccountId = 1, Name = "Manager 6" },
            new ManagerDto() { Id = 7, AccountId = 1, Name = "Manager 7" },
            new ManagerDto() { Id = 8, AccountId = 3, Name = "Manager 8" }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var managerRepository = new ManagerRepository(_mockContext.Object);

        // Act
        var result = managerRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 3);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(6, result[1].Id);
        Assert.Equal(7, result[2].Id);
    }
}
