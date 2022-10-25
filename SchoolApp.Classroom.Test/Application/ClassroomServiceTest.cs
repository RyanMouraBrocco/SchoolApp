using Moq;
using SchoolApp.Classroom.Application.Domain.Dtos;
using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;
using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Application.Domain.Entities.Subjects;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Classroom.Test.Application;

public class ClassroomServiceTest
{
    private readonly Mock<IClassroomRepository> _mockClassroomRepository;
    private readonly Mock<IClassroomStudentRepository> _mockClassroomStudentRepository;
    private readonly Mock<ISubjectRepository> _mockSubjectRepository;
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<ITeacherRepository> _mockTeacherRepository;

    public ClassroomServiceTest()
    {
        _mockClassroomRepository = new Mock<IClassroomRepository>();
        _mockClassroomStudentRepository = new Mock<IClassroomStudentRepository>();
        _mockSubjectRepository = new Mock<ISubjectRepository>();
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockTeacherRepository = new Mock<ITeacherRepository>();

        _mockClassroomRepository.Setup(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>())).Returns((Classroom.Application.Domain.Entities.Classrooms.Classroom x) => { x.Id = 1; return Task.FromResult(x); });
        _mockClassroomRepository.Setup(x => x.UpdateAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>())).Returns((Classroom.Application.Domain.Entities.Classrooms.Classroom x) => { return Task.FromResult(x); });
        _mockClassroomRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));
    }

    [Fact]
    public async Task CreateNewClassroom_SucessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 1, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act
        var result = await classroomService.CreateAsync(requesterUser, newClassroom);

        // Assert
        Assert.Equal(newClassroom.SubjectId, result.SubjectId);
        Assert.Equal(newClassroom.TeacherId, result.TeacherId);
        Assert.Equal(newClassroom.RoomNumber, result.RoomNumber);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.True(result.Id > 0);
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateNewClassroom_TryToCreateWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 1, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.CreateAsync(requesterUser, newClassroom));
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task CreateNewClassroom_TryToCreateWithNonExistentSubjectAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Subject>(null);
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.CreateAsync(requesterUser, newClassroom));
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task CreateNewClassroom_TryToCreateWithSubjectOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 2, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.CreateAsync(requesterUser, newClassroom));
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task CreateNewClassroom_TryToCreateWithNonExistentTeacherAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 1, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult((TeacherDto)null));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.CreateAsync(requesterUser, newClassroom));
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task CreateNewClassroom_TryToCreateWithTeacherOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 1, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 2 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.CreateAsync(requesterUser, newClassroom));
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task UpdateClassroom_SucessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 2,
            TeacherId = 2
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);
        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 1, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act
        var result = await classroomService.UpdateAsync(requesterUser, 1, newClassroom);

        // Assert
        Assert.Equal(newClassroom.SubjectId, result.SubjectId);
        Assert.Equal(newClassroom.TeacherId, result.TeacherId);
        Assert.Equal(newClassroom.RoomNumber, result.RoomNumber);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(classroomInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(classroomInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(classroomInDatabase.Id, result.Id);
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.UpdateAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateClassroom_TryToUpdateWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            TeacherId = 1
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);
        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 1, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.UpdateAsync(requesterUser, 1, newClassroom));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task UpdateClassroom_TryToUpdateWithNonExistentClassroomAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Classroom.Application.Domain.Entities.Classrooms.Classroom>(null);
        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Subject>(null);
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.UpdateAsync(requesterUser, 1, newClassroom));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task UpdateClassroom_TryToUpdateWithClassroomOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 2,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            TeacherId = 1
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);
        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 2, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.UpdateAsync(requesterUser, 1, newClassroom));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task UpdateClassroom_TryToUpdateWithNonExistentSubjectAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            TeacherId = 1
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);
        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Subject>(null);
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.UpdateAsync(requesterUser, 1, newClassroom));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task UpdateClassroom_TryToUpdateWithSubjectOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            TeacherId = 1
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);
        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 2, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 1 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.UpdateAsync(requesterUser, 1, newClassroom));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task UpdateClassroom_TryToUpdateWithNonExistentTeacherAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            TeacherId = 1
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);
        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 1, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult((TeacherDto)null));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.UpdateAsync(requesterUser, 1, newClassroom));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task UpdateClassroom_TryToUpdateWithTeacherOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            TeacherId = 1
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);
        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Subject() { Id = 1, AccountId = 1, Name = "Subject 1" });
        _mockTeacherRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TeacherDto() { Id = 1, AccountId = 2 }));
        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Student() { Id = 1, AccountId = 1, Name = "Student 1" });

        var newClassroom = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            SubjectId = 1,
            TeacherId = 1,
            RoomNumber = "1234",
            Students = new List<ClassroomStudent>() { new ClassroomStudent() { StudentId = 1 } }
        };

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.UpdateAsync(requesterUser, 1, newClassroom));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.InsertAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.DeleteAllByClassroomIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentRepository.Verify(x => x.InsertAsync(It.IsAny<ClassroomStudent>()), Times.Never);
    }

    [Fact]
    public async Task DeleteClassroom_SucessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 2,
            TeacherId = 2
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act
        await classroomService.DeleteAsync(requesterUser, 1);

        // Assert
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.UpdateAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteClassroom_TryToDeleteWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            TeacherId = 1
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.DeleteAsync(requesterUser, 1));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.UpdateAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteClassroom_TryToDeleteWithNonExistentClassroomAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Classroom.Application.Domain.Entities.Classrooms.Classroom>(null);

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.DeleteAsync(requesterUser, 1));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.UpdateAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteClassroom_TryToDeleteWithClassroomOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var classroomInDatabase = new Classroom.Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = 1,
            AccountId = 2,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            RoomNumber = "1234",
            SubjectId = 1,
            TeacherId = 1
        };

        _mockClassroomRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomInDatabase);

        var classroomService = new ClassroomService(_mockClassroomRepository.Object, _mockClassroomStudentRepository.Object, _mockSubjectRepository.Object, _mockStudentRepository.Object, _mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomService.DeleteAsync(requesterUser, 1));
        _mockClassroomRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.UpdateAsync(It.IsAny<Classroom.Application.Domain.Entities.Classrooms.Classroom>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

}
