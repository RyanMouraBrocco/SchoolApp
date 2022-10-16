using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;
using SchoolApp.IdentityProvider.Application.Services;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Functions;
using SchoolApp.IdentityProvider.Sql.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolApp.IdentityProvider.Test.Repositories.Base;

namespace SchoolApp.IdentityProvider.Test.Repositories;

public class FuctionRepositoryTest : BaseRepositoryTest<FunctionDto, Function, SchoolAppIdentityProviderContext, FunctionRepository>
{
    public FuctionRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new Function()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Description = "Description test",
            Name = "Name test",
            UpdaterId = null,
            UpdateDate = null
        };

        FunctionRepository functionRepository = new FunctionRepository(_mockContext.Object);

        await InternalInsertTestAsync(functionRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new Function()
        {
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = "Description test",
            Name = "Name test",
            UpdaterId = 2,
            UpdateDate = DateTime.Now,
            Id = 1
        };

        FunctionRepository functionRepository = new FunctionRepository(_mockContext.Object);

        await InternalUpdateTestAsync(functionRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<FunctionDto>
        {
            new FunctionDto { Name = "Func 1", Id = 1, AccountId = 2, Description = "Description test" },
        }.AsQueryable();

        FunctionRepository functionRepository = new FunctionRepository(_mockContext.Object);

        await InternalDeleteTestAsync(functionRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        FunctionRepository functionRepository = new FunctionRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(functionRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<FunctionDto>()
        {
            new FunctionDto() { Id = 1, AccountId = 1, Name = "Func 1"},
            new FunctionDto() { Id = 2, AccountId = 2, Name = "Func 2"},
        }.AsQueryable();
        FunctionRepository functionRepository = new FunctionRepository(_mockContext.Object);

        InternalGetOneByIdTest(functionRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        FunctionRepository functionRepository = new FunctionRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(functionRepository, 3);
    }
}
