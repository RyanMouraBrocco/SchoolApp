using SchoolApp.Classroom.Application.Domain.Entities.Subjects;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Subjects;
using SchoolApp.Classroom.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.Classroom.Test.Repositories;

public class SubjectRepositoryTest : BaseMainEntityRepositoryTest<SubjectDto, Subject, SchoolAppClassroomContext, SubjectRepository>
{
    public SubjectRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new Subject()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "test",
            UpdaterId = null,
            UpdateDate = null
        };

        SubjectRepository subjectRepository = new SubjectRepository(_mockContext.Object);

        await InternalInsertTestAsync(subjectRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new Subject()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "test",
            UpdaterId = null,
            UpdateDate = null
        };

        SubjectRepository subjectRepository = new SubjectRepository(_mockContext.Object);

        await InternalUpdateTestAsync(subjectRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<SubjectDto>
        {
            new SubjectDto {  Id = 1, AccountId = 2 },
        }.AsQueryable();

        SubjectRepository subjectRepository = new SubjectRepository(_mockContext.Object);

        await InternalDeleteTestAsync(subjectRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        SubjectRepository subjectRepository = new SubjectRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(subjectRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<SubjectDto>()
        {
            new SubjectDto() { Id = 1, AccountId = 1 },
            new SubjectDto() { Id = 2, AccountId = 2 },
        }.AsQueryable();
        SubjectRepository subjectRepository = new SubjectRepository(_mockContext.Object);

        InternalGetOneByIdTest(subjectRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        SubjectRepository subjectRepository = new SubjectRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(subjectRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<SubjectDto>()
        {
            new SubjectDto() { Id = 1, AccountId = 1  },
            new SubjectDto() { Id = 2, AccountId = 2  },
            new SubjectDto() { Id = 3, AccountId = 1  },
            new SubjectDto() { Id = 4, AccountId = 3  },
            new SubjectDto() { Id = 5, AccountId = 4  },
            new SubjectDto() { Id = 6, AccountId = 1  },
            new SubjectDto() { Id = 7, AccountId = 1  },
            new SubjectDto() { Id = 8, AccountId = 3  }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var subjectRepository = new SubjectRepository(_mockContext.Object);

        // Act
        var result = subjectRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }
}
