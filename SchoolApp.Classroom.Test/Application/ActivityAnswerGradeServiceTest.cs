using Moq;
using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.Classroom.Test.Application;

public class ActivityAnswerGradeServiceTest
{
    private readonly Mock<IActivityAnswerGradeRepository> _mockActivityAnswerGradeRepository;
    private readonly Mock<IClassroomRepository> _mockClassroomRepository;
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<IActivityAnswerRepository> _mockActivityAnswerRepository;

    public ActivityAnswerGradeServiceTest()
    {
        _mockActivityAnswerGradeRepository = new Mock<IActivityAnswerGradeRepository>();
        _mockClassroomRepository = new Mock<IClassroomRepository>();
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockActivityAnswerRepository = new Mock<IActivityAnswerRepository>();

        _mockActivityAnswerGradeRepository.Setup(x => x.InsertAsync(It.IsAny<ActivityAnswerGrade>())).Returns((ActivityAnswerGrade x) => { x.Id = 1; return Task.FromResult(x); });
        _mockActivityAnswerGradeRepository.Setup(x => x.UpdateAsync(It.IsAny<ActivityAnswerGrade>())).Returns((ActivityAnswerGrade x) => { return Task.FromResult(x); });
        _mockActivityAnswerGradeRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));
    }

    public async Task CreateActivityAnswerGrade_SucessfullyAsync()
    {
        var requesterUser = new AuthenticatedUserObject();


    }
}
