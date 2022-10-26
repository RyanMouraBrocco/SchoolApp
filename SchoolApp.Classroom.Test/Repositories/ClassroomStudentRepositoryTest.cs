using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Classrooms;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Classroom.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.Classroom.Test.Repositories;

public class ClassroomStudentRepositoryTest : BaseRepositoryTest<ClassroomStudentDto, ClassroomStudent, SchoolAppClassroomContext, ClassroomStudentRepository>
{
    public ClassroomStudentRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new ClassroomStudent()
        {
            StudentId = 1,
            ClassroomId = 1
        };

        ClassroomStudentRepository classroomStudentRepository = new ClassroomStudentRepository(_mockContext.Object);

        await InternalInsertTestAsync(classroomStudentRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new ClassroomStudent()
        {
            StudentId = 1,
            ClassroomId = 1
        };

        ClassroomStudentRepository classroomStudentRepository = new ClassroomStudentRepository(_mockContext.Object);

        await InternalUpdateTestAsync(classroomStudentRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<ClassroomStudentDto>
        {
            new ClassroomStudentDto {  Id = 1 },
        }.AsQueryable();

        ClassroomStudentRepository classroomStudentRepository = new ClassroomStudentRepository(_mockContext.Object);

        await InternalDeleteTestAsync(classroomStudentRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        ClassroomStudentRepository classroomStudentRepository = new ClassroomStudentRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(classroomStudentRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<ClassroomStudentDto>()
        {
            new ClassroomStudentDto() { Id = 1 },
            new ClassroomStudentDto() { Id = 2 },
        }.AsQueryable();
        ClassroomStudentRepository classroomStudentRepository = new ClassroomStudentRepository(_mockContext.Object);

        InternalGetOneByIdTest(classroomStudentRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        ClassroomStudentRepository classroomStudentRepository = new ClassroomStudentRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(classroomStudentRepository, 3);
    }

    [Fact]
    public void GetOneByClassroomIdAndOwnerIdTest()
    {
        var data = new List<ClassroomStudentDto>()
        {
            new ClassroomStudentDto() { Id = 1, ClassroomId = 1, StudentId = 1, Student = new () { Owners = new List<OwnerStudentDto>() } },
            new ClassroomStudentDto() { Id = 2, ClassroomId = 1, StudentId = 2, Student = new () { Owners = new List<OwnerStudentDto>() { new() { OwnerId  = 3}} }  },
            new ClassroomStudentDto() { Id = 3, ClassroomId = 1, StudentId = 3, Student = new () { Owners = new List<OwnerStudentDto>() { new() { OwnerId  = 1}} }  }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var classroomStudentRepository = new ClassroomStudentRepository(_mockContext.Object);

        // Act
        var result = classroomStudentRepository.GetOneByClassroomIdAndOwnerId(1, 1);

        Assert.Equal(3, result.Id);
    }
}
