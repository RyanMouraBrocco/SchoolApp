using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Classrooms;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Classroom.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.Classroom.Test.Repositories;

public class StudentRepositoryTest : BaseMainEntityRepositoryTest<StudentDto, Student, SchoolAppClassroomContext, StudentRepository>
{
    public StudentRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new Student()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "test",
            UpdaterId = null,
            UpdateDate = null
        };

        StudentRepository studentRepository = new StudentRepository(_mockContext.Object);

        await InternalInsertTestAsync(studentRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new Student()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "test",
            UpdaterId = null,
            UpdateDate = null
        };

        StudentRepository studentRepository = new StudentRepository(_mockContext.Object);

        await InternalUpdateTestAsync(studentRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<StudentDto>
        {
            new StudentDto {  Id = 1, AccountId = 2 },
        }.AsQueryable();

        StudentRepository studentRepository = new StudentRepository(_mockContext.Object);

        await InternalDeleteTestAsync(studentRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        StudentRepository studentRepository = new StudentRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(studentRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<StudentDto>()
        {
            new StudentDto() { Id = 1, AccountId = 1 },
            new StudentDto() { Id = 2, AccountId = 2 },
        }.AsQueryable();
        StudentRepository studentRepository = new StudentRepository(_mockContext.Object);

        InternalGetOneByIdTest(studentRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        StudentRepository studentRepository = new StudentRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(studentRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<StudentDto>()
        {
            new StudentDto() { Id = 1, AccountId = 1  },
            new StudentDto() { Id = 2, AccountId = 2  },
            new StudentDto() { Id = 3, AccountId = 1  },
            new StudentDto() { Id = 4, AccountId = 3  },
            new StudentDto() { Id = 5, AccountId = 4  },
            new StudentDto() { Id = 6, AccountId = 1  },
            new StudentDto() { Id = 7, AccountId = 1  },
            new StudentDto() { Id = 8, AccountId = 3  }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var studentRepository = new StudentRepository(_mockContext.Object);

        // Act
        var result = studentRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }

    [Fact]
    public void GetAllByOwnerIdTest()
    {
        var data = new List<StudentDto>()
        {
            new StudentDto() { Id = 1, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 2, AccountId = 2, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 2} }  },
            new StudentDto() { Id = 3, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 4, AccountId = 3, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 3} }  },
            new StudentDto() { Id = 5, AccountId = 4, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 4} }  },
            new StudentDto() { Id = 6, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 7, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 8, AccountId = 3, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 3} }  },
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var studentRepository = new StudentRepository(_mockContext.Object);

        // Act
        var result = studentRepository.GetAllByOwnerId(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }

    [Fact]
    public void GetOneByOwnerIdTest_Found()
    {
        var data = new List<StudentDto>()
        {
            new StudentDto() { Id = 1, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 2, AccountId = 2, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 2} }  },
            new StudentDto() { Id = 3, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 4, AccountId = 3, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 3} }  },
            new StudentDto() { Id = 5, AccountId = 4, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 4} }  },
            new StudentDto() { Id = 6, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 7, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 8, AccountId = 3, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 3} }  },
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var studentRepository = new StudentRepository(_mockContext.Object);

        // Act
        var result = studentRepository.GetOneByIdAndOwnerId(1, 1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void GetOneByOwnerIdTest_NotFound()
    {
        var data = new List<StudentDto>()
        {
            new StudentDto() { Id = 1, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 2, AccountId = 2, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 2} }  },
            new StudentDto() { Id = 3, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 4, AccountId = 3, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 3} }  },
            new StudentDto() { Id = 5, AccountId = 4, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 4} }  },
            new StudentDto() { Id = 6, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 7, AccountId = 1, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 1} }  },
            new StudentDto() { Id = 8, AccountId = 3, Owners = new List<OwnerStudentDto>() { new() { OwnerId = 3} }  },
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var studentRepository = new StudentRepository(_mockContext.Object);

        // Act
        var result = studentRepository.GetOneByIdAndOwnerId(1, 2);

        Assert.Null(result);
    }

    [Fact]
    public void GetAllByTeacherIdTest()
    {
        var data = new List<StudentDto>()
        {
            new StudentDto() { Id = 1, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 2, AccountId = 2, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 2 } } } },
            new StudentDto() { Id = 3, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 4, AccountId = 3, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 3 } } } },
            new StudentDto() { Id = 5, AccountId = 4, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 4 } } } },
            new StudentDto() { Id = 6, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 7, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 8, AccountId = 3, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 3 } } } },
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var studentRepository = new StudentRepository(_mockContext.Object);

        // Act
        var result = studentRepository.GetAllByTeacherId(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }

    [Fact]
    public void GetOneByTeacherIdTest_Found()
    {
        var data = new List<StudentDto>()
        {
            new StudentDto() { Id = 1, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 2, AccountId = 2, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 2 } } } },
            new StudentDto() { Id = 3, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 4, AccountId = 3, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 3 } } } },
            new StudentDto() { Id = 5, AccountId = 4, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 4 } } } },
            new StudentDto() { Id = 6, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 7, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 8, AccountId = 3, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 3 } } } },
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var studentRepository = new StudentRepository(_mockContext.Object);

        // Act
        var result = studentRepository.GetOneByIdAndTeacherId(1, 1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void GetOneByTeacherIdTest_NotFound()
    {
        var data = new List<StudentDto>()
        {
            new StudentDto() { Id = 1, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 2, AccountId = 2, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 2 } } } },
            new StudentDto() { Id = 3, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 4, AccountId = 3, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 3 } } } },
            new StudentDto() { Id = 5, AccountId = 4, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 4 } } } },
            new StudentDto() { Id = 6, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 7, AccountId = 1, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 1 } } } },
            new StudentDto() { Id = 8, AccountId = 3, Classrooms = new List<ClassroomStudentDto>() { new() { Classroom = new() { TeacherId = 3 } } } },
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var studentRepository = new StudentRepository(_mockContext.Object);

        // Act
        var result = studentRepository.GetOneByIdAndTeacherId(1, 2);

        Assert.Null(result);
    }
}
