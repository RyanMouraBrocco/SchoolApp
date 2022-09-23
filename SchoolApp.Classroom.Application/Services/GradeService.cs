using Microsoft.VisualBasic.CompilerServices;
using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Interfaces;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.Classroom.Application.Services;

public abstract class GradeService<TEntity> : IGradeService<TEntity> where TEntity : Grade
{
    protected readonly IGradeRepository<TEntity> _gradeRepository;
    private readonly IStudentService _studentService;

    public GradeService(IGradeRepository<TEntity> gradeRepository, IStudentService studentService)
    {
        _gradeRepository = gradeRepository;
        _studentService = studentService;
    }

    public virtual async Task<TEntity> CreateAsync(AuthenticatedUserObject requesterUser, TEntity newGrade)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        var studentCheck = _studentService.GetOneById(requesterUser, newGrade.StudentId);
        if (studentCheck == null)
            throw new UnauthorizedAccessException("Student not found");

        if (newGrade.Value < 0)
            newGrade.Value = 0;

        newGrade.AccountId = requesterUser.AccountId;
        newGrade.CreatorId = requesterUser.UserId;
        newGrade.CreationDate = DateTime.Now;
        newGrade.UpdaterId = null;
        newGrade.UpdateDate = null;

        return await _gradeRepository.InsertAsync(newGrade);
    }

    public virtual async Task<TEntity> UpdateAsync(AuthenticatedUserObject requesterUser, int gradeId, TEntity updatedGrade)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        var gradeCheck = _gradeRepository.GetOneById(gradeId);
        if (gradeCheck == null || gradeCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Grade not found");

        if (updatedGrade.Value < 0)
            updatedGrade.Value = 0;

        updatedGrade.Id = gradeId;
        updatedGrade.AccountId = gradeCheck.AccountId;
        updatedGrade.CreatorId = gradeCheck.CreatorId;
        updatedGrade.CreationDate = gradeCheck.CreationDate;
        updatedGrade.StudentId = gradeCheck.StudentId;
        updatedGrade.UpdaterId = requesterUser.UserId;
        updatedGrade.UpdateDate = DateTime.Now;

        return await _gradeRepository.UpdateAsync(updatedGrade);
    }

    public virtual async Task DeleteAsync(AuthenticatedUserObject requesterUser, int gradeId)
    {
        GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        var gradeCheck = _gradeRepository.GetOneById(gradeId);
        if (gradeCheck == null || gradeCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Grade not found");

        gradeCheck.UpdateDate = DateTime.Now;
        gradeCheck.UpdaterId = requesterUser.UserId;

        await _gradeRepository.UpdateAsync(gradeCheck);
        await _gradeRepository.DeleteAsync(gradeId);
    }

    public IList<TEntity> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Manager => new List<TEntity>(),
            UserTypeEnum.Teacher => new List<TEntity>(),
            UserTypeEnum.Owner => new List<TEntity>(),
            _ => throw new NotImplementedException("User not valid")
        };
    }
}
