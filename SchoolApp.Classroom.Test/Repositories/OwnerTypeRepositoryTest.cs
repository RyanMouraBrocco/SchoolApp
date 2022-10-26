using SchoolApp.Classroom.Application.Domain.Entities.OwnerTypes;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.OwnerTypes;
using SchoolApp.Classroom.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.Classroom.Test.Repositories;

public class OwnerTypeRepositoryTest : BaseMainEntityRepositoryTest<OwnerTypeDto, OwnerType, SchoolAppClassroomContext, OwnerTypeRepository>
{
    public OwnerTypeRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new OwnerType()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "test",
            UpdaterId = null,
            UpdateDate = null
        };

        OwnerTypeRepository ownerTypeRepository = new OwnerTypeRepository(_mockContext.Object);

        await InternalInsertTestAsync(ownerTypeRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new OwnerType()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "test",
            UpdaterId = null,
            UpdateDate = null
        };

        OwnerTypeRepository ownerTypeRepository = new OwnerTypeRepository(_mockContext.Object);

        await InternalUpdateTestAsync(ownerTypeRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<OwnerTypeDto>
        {
            new OwnerTypeDto {  Id = 1, AccountId = 2 },
        }.AsQueryable();

        OwnerTypeRepository ownerTypeRepository = new OwnerTypeRepository(_mockContext.Object);

        await InternalDeleteTestAsync(ownerTypeRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        OwnerTypeRepository ownerTypeRepository = new OwnerTypeRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(ownerTypeRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<OwnerTypeDto>()
        {
            new OwnerTypeDto() { Id = 1, AccountId = 1 },
            new OwnerTypeDto() { Id = 2, AccountId = 2 },
        }.AsQueryable();
        OwnerTypeRepository ownerTypeRepository = new OwnerTypeRepository(_mockContext.Object);

        InternalGetOneByIdTest(ownerTypeRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        OwnerTypeRepository ownerTypeRepository = new OwnerTypeRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(ownerTypeRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<OwnerTypeDto>()
        {
            new OwnerTypeDto() { Id = 1, AccountId = 1  },
            new OwnerTypeDto() { Id = 2, AccountId = 2  },
            new OwnerTypeDto() { Id = 3, AccountId = 1  },
            new OwnerTypeDto() { Id = 4, AccountId = 3  },
            new OwnerTypeDto() { Id = 5, AccountId = 4  },
            new OwnerTypeDto() { Id = 6, AccountId = 1  },
            new OwnerTypeDto() { Id = 7, AccountId = 1  },
            new OwnerTypeDto() { Id = 8, AccountId = 3  }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var ownerTypeRepository = new OwnerTypeRepository(_mockContext.Object);

        // Act
        var result = ownerTypeRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }
}
