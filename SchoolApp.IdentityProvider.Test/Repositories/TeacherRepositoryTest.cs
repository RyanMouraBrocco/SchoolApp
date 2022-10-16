using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Repositories;
using SchoolApp.IdentityProvider.Test.Repositories.Base;

namespace SchoolApp.IdentityProvider.Test.Repositories;

public class TeacherRepositoryTest : BaseMainEntityRepositoryTest<TeacherDto, Teacher, SchoolAppIdentityProviderContext, TeacherRepository>
{
    public TeacherRepositoryTest() : base()
    {
    }

    [Fact]
    public async Task InsertTestAsync()
    {
        // Arrange
        var newItem = new Teacher()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "Name test",
            DocumentId = "Document",
            Email = "e@e.com",
            Password = "HASH PASSWORD",
            HiringDate = DateTime.Now,
            Salary = 200,
            UpdaterId = null,
            UpdateDate = null
        };

        TeacherRepository teacherRepository = new TeacherRepository(_mockContext.Object);

        await InternalInsertTestAsync(teacherRepository, newItem);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var newItem = new Teacher()
        {
            AccountId = 1,
            CreationDate = DateTime.Now,
            CreatorId = 1,
            Name = "Name test",
            DocumentId = "Document",
            Email = "e@e.com",
            Password = "HASH PASSWORD",
            HiringDate = DateTime.Now,
            Salary = 200,
            UpdaterId = null,
            UpdateDate = null,
            Id = 1
        };

        TeacherRepository teacherRepository = new TeacherRepository(_mockContext.Object);

        await InternalUpdateTestAsync(teacherRepository, newItem);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        var data = new List<TeacherDto>
        {
            new TeacherDto { Name = "Func 1", Id = 1, AccountId = 2 },
        }.AsQueryable();

        TeacherRepository teacherRepository = new TeacherRepository(_mockContext.Object);

        await InternalDeleteTestAsync(teacherRepository, data);
    }

    [Fact]
    public async Task DeleteTest_TryToDeleteANonExistentItemAsync()
    {
        // Arrange
        TeacherRepository teacherRepository = new TeacherRepository(_mockContext.Object);

        await InternalDeleteTest_TryToDeleteANonExistentItemAsync(teacherRepository);
    }

    [Fact]
    public void GetOneByIdTest()
    {
        // Arrange
        var data = new List<TeacherDto>()
        {
            new TeacherDto() { Id = 1, AccountId = 1, Name = "Teacher 1"},
            new TeacherDto() { Id = 2, AccountId = 2, Name = "Teacher 2"},
        }.AsQueryable();
        TeacherRepository teacherRepository = new TeacherRepository(_mockContext.Object);

        InternalGetOneByIdTest(teacherRepository, data, 1);
    }

    [Fact]
    public void GetOneByIdTest_NotFound()
    {
        // Arrange
        TeacherRepository teacherRepository = new TeacherRepository(_mockContext.Object);

        InternalGetOneByIdTest_NotFound(teacherRepository, 3);
    }

    [Fact]
    public void GetAllTest()
    {
        var data = new List<TeacherDto>()
        {
            new TeacherDto() { Id = 1, AccountId = 1, Name = "Teacher 1" },
            new TeacherDto() { Id = 2, AccountId = 2, Name = "Teacher 2" },
            new TeacherDto() { Id = 3, AccountId = 1, Name = "Teacher 3", Deleted = true },
            new TeacherDto() { Id = 4, AccountId = 3, Name = "Teacher 4" },
            new TeacherDto() { Id = 5, AccountId = 4, Name = "Teacher 5" },
            new TeacherDto() { Id = 6, AccountId = 1, Name = "Teacher 6" },
            new TeacherDto() { Id = 7, AccountId = 1, Name = "Teacher 7" },
            new TeacherDto() { Id = 8, AccountId = 3, Name = "Teacher 8" }
        }.AsQueryable();

        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);
        var teacherRepository = new TeacherRepository(_mockContext.Object);

        // Act
        var result = teacherRepository.GetAll(1, 100, 0);

        Assert.True(result.Count == 3);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(6, result[1].Id);
        Assert.Equal(7, result[2].Id);
    }
}
