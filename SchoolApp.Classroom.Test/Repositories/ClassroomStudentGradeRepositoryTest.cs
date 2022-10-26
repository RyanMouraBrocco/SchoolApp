using System.Net.Mime;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Grades;
using SchoolApp.Classroom.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;
using SchoolApp.Classroom.Application.Domain.Entities.Grades;


namespace SchoolApp.Classroom.Test.Repositories;

public class ClassroomStudentGradeStudentGradeRepositoryTest : BaseMainEntityRepositoryTest<ClassroomStudentGradeDto, ClassroomStudentGrade, SchoolAppClassroomContext, ClassroomStudentGradeRepository>
{
    public ClassroomStudentGradeStudentGradeRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new ClassroomStudentGrade()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            ClassroomStudentId = 1,
            StudentId = 1,
            UpdaterId = null,
            UpdateDate = null
        };

        ClassroomStudentGradeRepository classroomStudentGradeRepository = new ClassroomStudentGradeRepository(_mockContext.Object);

        await InternalInsertTestAsync(classroomStudentGradeRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new ClassroomStudentGrade()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            ClassroomStudentId = 1,
            StudentId = 1,
            UpdaterId = null,
            UpdateDate = null
        };

        ClassroomStudentGradeRepository classroomStudentGradeRepository = new ClassroomStudentGradeRepository(_mockContext.Object);

        await InternalUpdateTestAsync(classroomStudentGradeRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<ClassroomStudentGradeDto>
        {
            new ClassroomStudentGradeDto {  Id = 1, AccountId = 2 },
        }.AsQueryable();

        ClassroomStudentGradeRepository classroomStudentGradeRepository = new ClassroomStudentGradeRepository(_mockContext.Object);

        await InternalDeleteTestAsync(classroomStudentGradeRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        ClassroomStudentGradeRepository classroomStudentGradeRepository = new ClassroomStudentGradeRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(classroomStudentGradeRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<ClassroomStudentGradeDto>()
        {
            new ClassroomStudentGradeDto() { Id = 1, AccountId = 1 },
            new ClassroomStudentGradeDto() { Id = 2, AccountId = 2 },
        }.AsQueryable();
        ClassroomStudentGradeRepository classroomStudentGradeRepository = new ClassroomStudentGradeRepository(_mockContext.Object);

        InternalGetOneByIdTest(classroomStudentGradeRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        ClassroomStudentGradeRepository classroomStudentGradeRepository = new ClassroomStudentGradeRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(classroomStudentGradeRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<ClassroomStudentGradeDto>()
        {
            new ClassroomStudentGradeDto() { Id = 1, AccountId = 1  },
            new ClassroomStudentGradeDto() { Id = 2, AccountId = 2  },
            new ClassroomStudentGradeDto() { Id = 3, AccountId = 1  },
            new ClassroomStudentGradeDto() { Id = 4, AccountId = 3  },
            new ClassroomStudentGradeDto() { Id = 5, AccountId = 4  },
            new ClassroomStudentGradeDto() { Id = 6, AccountId = 1  },
            new ClassroomStudentGradeDto() { Id = 7, AccountId = 1  },
            new ClassroomStudentGradeDto() { Id = 8, AccountId = 3  }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var classroomStudentGradeRepository = new ClassroomStudentGradeRepository(_mockContext.Object);

        // Act
        var result = classroomStudentGradeRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }
}
