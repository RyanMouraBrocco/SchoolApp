using Moq;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Formation;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.IdentityProvider.Test.Application;

public class TeacherServiceTest
{
    private readonly Mock<ITeacherRepository> _mockTeacherRepository;
    private readonly Mock<ITeacherFormationRepository> _mockTeacherFormationRepository;

    public TeacherServiceTest()
    {
        _mockTeacherRepository = new Mock<ITeacherRepository>();
        _mockTeacherFormationRepository = new Mock<ITeacherFormationRepository>();

        _mockTeacherRepository.Setup(x => x.InsertAsync(It.IsAny<Teacher>())).Returns((Teacher x) => { x.Id = 1; return Task.FromResult(x); });
        _mockTeacherRepository.Setup(x => x.UpdateAsync(It.IsAny<Teacher>())).Returns((Teacher x) => { return Task.FromResult(x); });
        _mockTeacherRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));

        _mockTeacherFormationRepository.Setup(x => x.InsertAsync(It.IsAny<TeacherFormation>())).Returns((TeacherFormation x) => { x.Id = 1; return Task.FromResult(x); });
        _mockTeacherFormationRepository.Setup(x => x.DeleteAllByTeacherIdAsync(It.IsAny<int>()));
    }

    [Fact]
    public async Task CreateNewTeacher_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var purePassword = "1StrongP4ssw0rd@";
        var newTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = purePassword,
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Teacher>(null);

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act
        var result = await teacherService.CreateAsync(requesterUser, newTeacher);

        // Assert
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(newTeacher.Id), Times.Once);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(newTeacher.Email), Times.Once);
        _mockTeacherRepository.Verify(x => x.InsertAsync(newTeacher), Times.Once);
        Assert.Equal(newTeacher.Name, result.Name);
        Assert.Equal(newTeacher.DocumentId, result.DocumentId);
        Assert.Equal(IdentityProvider.Application.Helpers.Utils.HashText(purePassword), result.Password);
        Assert.Equal(newTeacher.Email, result.Email);
        Assert.Equal(newTeacher.Salary, result.Salary);
        Assert.Equal(newTeacher.HiringDate, result.HiringDate);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Equal(newTeacher.AcademicFormation, result.AcademicFormation);
        Assert.Equal(newTeacher.Formations, result.Formations);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.True(result.Id > 0);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateNewTeacher_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var purePassword = "1StrongP4ssw0rd@";
        var newTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = purePassword,
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.CreateAsync(requesterUser, newTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(newTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(newTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.InsertAsync(newTeacher), Times.Never);
    }

    [Fact]
    public async Task CreateNewTeacher_TryToCreateWithoutFormationsAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var purePassword = "1StrongP4ssw0rd@";
        var newTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = purePassword,
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>()
        };

        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Teacher>(null);

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => teacherService.CreateAsync(requesterUser, newTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(newTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(newTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.InsertAsync(newTeacher), Times.Never);
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("12345678")]
    [InlineData("abcd")]
    [InlineData("abcdefgh")]
    [InlineData("ABCD")]
    [InlineData("ABCDEFGH")]
    [InlineData("aBcD")]
    [InlineData("aBcDeFgH")]
    [InlineData("4bCD")]
    public async Task CreateNewTeacher_TryToCreateWithoutASecurityPasswordAsync(string password)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = password,
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => teacherService.CreateAsync(requesterUser, newTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(newTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(newTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.InsertAsync(newTeacher), Times.Never);
    }

    [Theory]
    [InlineData("Name", null)]
    [InlineData("Name", "")]
    [InlineData("Name", " ")]
    [InlineData("Email", null)]
    [InlineData("Email", "")]
    [InlineData("Email", " ")]
    [InlineData("Password", null)]
    [InlineData("Password", "")]
    [InlineData("Password", " ")]
    [InlineData("DocumentId", null)]
    [InlineData("DocumentId", "")]
    [InlineData("DocumentId", " ")]
    public async Task CreateNewTeacher_TryToCreateWithInvalidValuesAsync(string property, object invalidValue)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };
        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        typeof(Teacher).GetProperty(property).SetValue(newTeacher, invalidValue);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => teacherService.CreateAsync(requesterUser, newTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(newTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(newTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.InsertAsync(newTeacher), Times.Never);
    }

    [Fact]
    public async Task CreateNewTeacher_TryToCreateNegativeSalaryAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = -200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };
        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => teacherService.CreateAsync(requesterUser, newTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(newTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(newTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.InsertAsync(newTeacher), Times.Never);
    }

    [Fact]
    public async Task CreateNewTeacher_TryToWithDuplicatedEmailAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(new Teacher() { Id = 2 });

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.CreateAsync(requesterUser, newTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(newTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(newTeacher.Email), Times.Once);
        _mockTeacherRepository.Verify(x => x.InsertAsync(newTeacher), Times.Never);
    }

    [Fact]
    public async Task UpdateTeacher_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var purePassword = "1StrongP4ssw0rd@";
        var updatedTeacher = new Teacher()
        {
            Name = "Name test 2",
            DocumentId = "My document 2 ",
            Password = purePassword + "2",
            Email = "e2@e.com",
            Salary = 201.00M,
            HiringDate = DateTime.Now.AddDays(1),
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good 2" } }
        };

        var teacherSaveInDatabase = new Teacher()
        {
            Id = 1,
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText(purePassword),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Teacher>(null);
        _mockTeacherRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(teacherSaveInDatabase);

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act
        var result = await teacherService.UpdateAsync(requesterUser, 1, updatedTeacher);

        // Assert
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(updatedTeacher.Id), Times.Once);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(updatedTeacher.Email), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(updatedTeacher), Times.Once);
        Assert.Equal(updatedTeacher.Name, result.Name);
        Assert.Equal(updatedTeacher.DocumentId, result.DocumentId);
        Assert.Equal(IdentityProvider.Application.Helpers.Utils.HashText(purePassword), result.Password);
        Assert.Equal(updatedTeacher.Email, result.Email);
        Assert.Equal(updatedTeacher.Salary, result.Salary);
        Assert.Equal(updatedTeacher.HiringDate, result.HiringDate);
        Assert.Equal(updatedTeacher.AcademicFormation, updatedTeacher.AcademicFormation);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(requesterUser.AccountId, teacherSaveInDatabase.AccountId);
        Assert.Equal(teacherSaveInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(teacherSaveInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(teacherSaveInDatabase.Id, result.Id);
        Assert.Equal(updatedTeacher.Formations, result.Formations);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateTeacher_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var purePassword = "1StrongP4ssw0rd@";
        var updatedTeacher = new Teacher()
        {
            Name = "Name test 2",
            DocumentId = "My document 2 ",
            Password = purePassword + "2",
            Email = "e2@e.com",
            Salary = 201.00M,
            HiringDate = DateTime.Now.AddDays(1),
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.UpdateAsync(requesterUser, 1, updatedTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(updatedTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(updatedTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(updatedTeacher), Times.Never);
    }

    [Theory]
    [InlineData("Name", null)]
    [InlineData("Name", "")]
    [InlineData("Name", " ")]
    [InlineData("Email", null)]
    [InlineData("Email", "")]
    [InlineData("Email", " ")]
    [InlineData("Password", null)]
    [InlineData("Password", "")]
    [InlineData("Password", " ")]
    [InlineData("DocumentId", null)]
    [InlineData("DocumentId", "")]
    [InlineData("DocumentId", " ")]
    public async Task UpdateTeacher_TryToUpdateWithInvalidValuesAsync(string property, object invalidValue)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedTeacher = new Teacher()
        {
            Name = "Name test 2",
            DocumentId = "My document 2",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 201.00M,
            HiringDate = DateTime.Now.AddDays(3),
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };
        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        typeof(Teacher).GetProperty(property).SetValue(updatedTeacher, invalidValue);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => teacherService.UpdateAsync(requesterUser, 1, updatedTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(updatedTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(updatedTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(updatedTeacher), Times.Never);
    }

    [Fact]
    public async Task UpdateTeacher_TryToUpdateToNegativeSalaryAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = -200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };
        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => teacherService.UpdateAsync(requesterUser, 1, updatedTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(updatedTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(updatedTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(updatedTeacher), Times.Never);
    }

    [Fact]
    public async Task UpdateTeacher_TryToUpdateWithDuplicatedEmailAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        var teacherSaveInDatabase = new Teacher()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } },
            Id = 1
        };

        _mockTeacherRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(teacherSaveInDatabase);
        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(new Teacher() { Id = 2 });

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.UpdateAsync(requesterUser, 1, updatedTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(updatedTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(updatedTeacher.Email), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(updatedTeacher), Times.Never);
    }

    [Fact]
    public async Task UpdateTeacher_TryToUpdateANonexistentTeacherAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        _mockTeacherRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Teacher>(null);

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.UpdateAsync(requesterUser, 1, updatedTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(updatedTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(updatedTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(updatedTeacher), Times.Never);
    }

    [Fact]
    public async Task UpdateTeacher_TryToUpdateTeacherOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        var teacherSaveInDatabase = new Teacher()
        {
            Name = "Name test",
            AccountId = 2,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } },
            Id = 1
        };

        _mockTeacherRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(teacherSaveInDatabase);

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.UpdateAsync(requesterUser, 1, updatedTeacher));
        _mockTeacherFormationRepository.Verify(x => x.DeleteAllByTeacherIdAsync(updatedTeacher.Id), Times.Never);
        _mockTeacherFormationRepository.Verify(x => x.InsertAsync(It.IsAny<TeacherFormation>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(updatedTeacher.Email), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(updatedTeacher), Times.Never);
    }

    [Fact]
    public async Task DeleteTeacher_SuccessfullyAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedTeacher = new Teacher()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } }
        };

        var teacherSaveInDatabase = new Teacher()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            AcademicFormation = "nothing",
            Formations = new List<TeacherFormation>() { new TeacherFormation() { AcademicFormation = "Nothing good" } },
            Id = 1
        };

        _mockTeacherRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(teacherSaveInDatabase);

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act
        await teacherService.DeleteAsync(requesterUser, 1);

        // Assert
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(It.IsAny<Teacher>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.DeleteAsync(1), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteTeacher_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.DeleteAsync(requesterUser, 1));
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(It.IsAny<Teacher>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }

    [Fact]
    public async Task DeleteTeacher_TryToDeleteANonexistentTeacherAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        _mockTeacherRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Teacher>(null);

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.DeleteAsync(requesterUser, 1));
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(It.IsAny<Teacher>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }

    [Fact]
    public async Task DeleteTeacher_TryToDeleteTeacherOfAnotherAccountAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        _mockTeacherRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Teacher() { Id = 1, AccountId = 2 });

        var teacherService = new TeacherService(_mockTeacherRepository.Object, _mockTeacherFormationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => teacherService.DeleteAsync(requesterUser, 1));
        _mockTeacherRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockTeacherRepository.Verify(x => x.UpdateAsync(It.IsAny<Teacher>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }

}
