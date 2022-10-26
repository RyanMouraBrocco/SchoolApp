using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Grades;
using SchoolApp.Classroom.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.Classroom.Test.Repositories;

public class ActivityAnswerGradeRepositoryTest : BaseMainEntityRepositoryTest<ActivityAnswerGradeDto, ActivityAnswerGrade, SchoolAppClassroomContext, ActivityAnswerGradeRepository>
{
    public ActivityAnswerGradeRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new ActivityAnswerGrade()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            ActivityAnswerId = "1",
            StudentId = 1,
            UpdaterId = null,
            UpdateDate = null
        };

        ActivityAnswerGradeRepository activityAnswerGradeRepository = new ActivityAnswerGradeRepository(_mockContext.Object);

        await InternalInsertTestAsync(activityAnswerGradeRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new ActivityAnswerGrade()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            ActivityAnswerId = "1",
            StudentId = 1,
            UpdaterId = null,
            UpdateDate = null
        };

        ActivityAnswerGradeRepository activityAnswerGradeRepository = new ActivityAnswerGradeRepository(_mockContext.Object);

        await InternalUpdateTestAsync(activityAnswerGradeRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<ActivityAnswerGradeDto>
        {
            new ActivityAnswerGradeDto {  Id = 1, AccountId = 2 },
        }.AsQueryable();

        ActivityAnswerGradeRepository activityAnswerGradeRepository = new ActivityAnswerGradeRepository(_mockContext.Object);

        await InternalDeleteTestAsync(activityAnswerGradeRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        ActivityAnswerGradeRepository activityAnswerGradeRepository = new ActivityAnswerGradeRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(activityAnswerGradeRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<ActivityAnswerGradeDto>()
        {
            new ActivityAnswerGradeDto() { Id = 1, AccountId = 1 },
            new ActivityAnswerGradeDto() { Id = 2, AccountId = 2 },
        }.AsQueryable();
        ActivityAnswerGradeRepository activityAnswerGradeRepository = new ActivityAnswerGradeRepository(_mockContext.Object);

        InternalGetOneByIdTest(activityAnswerGradeRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        ActivityAnswerGradeRepository activityAnswerGradeRepository = new ActivityAnswerGradeRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(activityAnswerGradeRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<ActivityAnswerGradeDto>()
        {
            new ActivityAnswerGradeDto() { Id = 1, AccountId = 1  },
            new ActivityAnswerGradeDto() { Id = 2, AccountId = 2  },
            new ActivityAnswerGradeDto() { Id = 3, AccountId = 1  },
            new ActivityAnswerGradeDto() { Id = 4, AccountId = 3  },
            new ActivityAnswerGradeDto() { Id = 5, AccountId = 4  },
            new ActivityAnswerGradeDto() { Id = 6, AccountId = 1  },
            new ActivityAnswerGradeDto() { Id = 7, AccountId = 1  },
            new ActivityAnswerGradeDto() { Id = 8, AccountId = 3  }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var activityAnswerGradeRepository = new ActivityAnswerGradeRepository(_mockContext.Object);

        // Act
        var result = activityAnswerGradeRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }
}
