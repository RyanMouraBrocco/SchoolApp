using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Classrooms;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Classroom.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.Classroom.Test.Repositories;

public class ClassroomRepositoryTest : BaseMainEntityRepositoryTest<ClassroomDto, Classroom.Application.Domain.Entities.Classrooms.Classroom, SchoolAppClassroomContext, ClassroomRepository>
{
    public ClassroomRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            UpdaterId = null,
            UpdateDate = null
        };

        ClassroomRepository classroomRepository = new ClassroomRepository(_mockContext.Object);

        await InternalInsertTestAsync(classroomRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            UpdaterId = null,
            UpdateDate = null
        };


        ClassroomRepository classroomRepository = new ClassroomRepository(_mockContext.Object);

        await InternalUpdateTestAsync(classroomRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<ClassroomDto>
        {
            new ClassroomDto {  Id = 1, AccountId = 2 },
        }.AsQueryable();

        ClassroomRepository classroomRepository = new ClassroomRepository(_mockContext.Object);

        await InternalDeleteTestAsync(classroomRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        ClassroomRepository classroomRepository = new ClassroomRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(classroomRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<ClassroomDto>()
        {
            new ClassroomDto() { Id = 1, AccountId = 1 },
            new ClassroomDto() { Id = 2, AccountId = 2 },
        }.AsQueryable();
        ClassroomRepository classroomRepository = new ClassroomRepository(_mockContext.Object);

        InternalGetOneByIdTest(classroomRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        ClassroomRepository classroomRepository = new ClassroomRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(classroomRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<ClassroomDto>()
        {
            new ClassroomDto() { Id = 1, AccountId = 1  },
            new ClassroomDto() { Id = 2, AccountId = 2  },
            new ClassroomDto() { Id = 3, AccountId = 1  },
            new ClassroomDto() { Id = 4, AccountId = 3  },
            new ClassroomDto() { Id = 5, AccountId = 4  },
            new ClassroomDto() { Id = 6, AccountId = 1  },
            new ClassroomDto() { Id = 7, AccountId = 1  },
            new ClassroomDto() { Id = 8, AccountId = 3  }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var classroomRepository = new ClassroomRepository(_mockContext.Object);

        // Act
        var result = classroomRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }

    [Fact]
    public void GetAllByTeacherIdTest()
    {
        var data = new List<ClassroomDto>()
        {
            new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1  },
            new ClassroomDto() { Id = 2, AccountId = 2, TeacherId = 4  },
            new ClassroomDto() { Id = 3, AccountId = 1, TeacherId = 1  },
            new ClassroomDto() { Id = 4, AccountId = 3, TeacherId = 3  },
            new ClassroomDto() { Id = 5, AccountId = 4, TeacherId = 5  },
            new ClassroomDto() { Id = 6, AccountId = 1, TeacherId = 1  },
            new ClassroomDto() { Id = 7, AccountId = 1, TeacherId = 1  },
            new ClassroomDto() { Id = 8, AccountId = 3, TeacherId = 4  }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var classroomRepository = new ClassroomRepository(_mockContext.Object);

        // Act
        var result = classroomRepository.GetAllByTeacherId(1, 100, 0);

        Assert.True(result.Count == 4);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[1].Id);
        Assert.Equal(6, result[2].Id);
        Assert.Equal(7, result[3].Id);
    }

    [Fact]
    public void GetAllByOwnerIdTest()
    {
        var data = new List<ClassroomDto>()
        {
            new ClassroomDto() { Id = 1, AccountId = 1, Students = new List<ClassroomStudentDto> () { new(){ Student = new() { Owners = new List<OwnerStudentDto>() { new() {  OwnerId = 1 } } } } } },
            new ClassroomDto() { Id = 2, AccountId = 2, Students = new List<ClassroomStudentDto> () { new(){ Student = new() { Owners = new List<OwnerStudentDto>() { new() {  OwnerId = 4 } } } } } },
            new ClassroomDto() { Id = 3, AccountId = 1, Students = new List<ClassroomStudentDto> () { new(){ Student = new() { Owners = new List<OwnerStudentDto>() { new() {  OwnerId = 2 } } } } } },
            new ClassroomDto() { Id = 4, AccountId = 3, Students = new List<ClassroomStudentDto> () { new(){ Student = new() { Owners = new List<OwnerStudentDto>() { new() {  OwnerId = 5 } } } } } },
            new ClassroomDto() { Id = 5, AccountId = 4, Students = new List<ClassroomStudentDto> () { new(){ Student = new() { Owners = new List<OwnerStudentDto>() { new() {  OwnerId = 6 } } } } } },
            new ClassroomDto() { Id = 6, AccountId = 1, Students = new List<ClassroomStudentDto> () { new(){ Student = new() { Owners = new List<OwnerStudentDto>() } } } },
            new ClassroomDto() { Id = 7, AccountId = 1, Students = new List<ClassroomStudentDto> () { new(){ Student = new() { Owners = new List<OwnerStudentDto>() { new() {  OwnerId = 2 }, new() {  OwnerId = 1 } } } } } },
            new ClassroomDto() { Id = 8, AccountId = 3, Students = new List<ClassroomStudentDto> () { new(){ Student = new() { Owners = new List<OwnerStudentDto>() { new() {  OwnerId = 4 } } } } } }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var classroomRepository = new ClassroomRepository(_mockContext.Object);

        // Act
        var result = classroomRepository.GetAllByOwnerId(1, 100, 0);

        Assert.True(result.Count == 2);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(7, result[1].Id);
    }
}
