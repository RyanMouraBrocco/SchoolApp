using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Classroom.Sql.Repositories;
using SchoolApp.Shared.Utils.Test.Repositories;

namespace SchoolApp.Classroom.Test.Repositories;

public class OwnerStudentRepositoryTest : BaseRepositoryTest<OwnerStudentDto, OwnerStudent, SchoolAppClassroomContext, OwnerStudentRepository>
{
    public OwnerStudentRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new OwnerStudent()
        {
            StudentId = 1,
            OwnerId = 1,
            OwnerTypeId = 1
        };

        OwnerStudentRepository ownerStudentRepository = new OwnerStudentRepository(_mockContext.Object);

        await InternalInsertTestAsync(ownerStudentRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new OwnerStudent()
        {
            StudentId = 1,
            OwnerId = 1,
            OwnerTypeId = 1
        };

        OwnerStudentRepository ownerStudentRepository = new OwnerStudentRepository(_mockContext.Object);

        await InternalUpdateTestAsync(ownerStudentRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<OwnerStudentDto>
        {
            new OwnerStudentDto {  Id = 1 },
        }.AsQueryable();

        OwnerStudentRepository ownerStudentRepository = new OwnerStudentRepository(_mockContext.Object);

        await InternalDeleteTestAsync(ownerStudentRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        OwnerStudentRepository ownerStudentRepository = new OwnerStudentRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(ownerStudentRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<OwnerStudentDto>()
        {
            new OwnerStudentDto() { Id = 1 },
            new OwnerStudentDto() { Id = 2 },
        }.AsQueryable();
        OwnerStudentRepository ownerStudentRepository = new OwnerStudentRepository(_mockContext.Object);

        InternalGetOneByIdTest(ownerStudentRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        OwnerStudentRepository ownerStudentRepository = new OwnerStudentRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(ownerStudentRepository, 3);
    }
}
