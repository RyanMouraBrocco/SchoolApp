using System.Reflection.PortableExecutable;
using SchoolApp.Classroom.Application.Domain.Entities.Subjects;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.Classroom.Application.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;

    public SubjectService(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<Subject> CreateAsync(AuthenticatedUserObject requesterUser, Subject newSubject)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        if (string.IsNullOrEmpty(newSubject.Name?.Trim()))
            throw new FormatException("Name can't not be null or empty");

        newSubject.AccountId = requesterUser.AccountId;
        newSubject.CreatorId = requesterUser.UserId;
        newSubject.CreationDate = DateTime.Now;
        newSubject.UpdateDate = null;
        newSubject.UpdaterId = null;

        return await _subjectRepository.InsertAsync(newSubject);
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, int itemId)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        var subjectCheck = _subjectRepository.GetOneById(itemId);
        if (subjectCheck != null || subjectCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Subject not found");

        subjectCheck.UpdateDate = DateTime.Now;
        subjectCheck.UpdaterId = requesterUser.UserId;

        await _subjectRepository.UpdateAsync(subjectCheck);
        await _subjectRepository.DeleteAsync(itemId);
    }

    public IList<Subject> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return _subjectRepository.GetAll(requesterUser.AccountId, top, skip);
    }

    public async Task<Subject> UpdateAsync(AuthenticatedUserObject requesterUser, int itemId, Subject updatedSubject)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        var subjectCheck = _subjectRepository.GetOneById(updatedSubject.Id);
        if (subjectCheck != null || subjectCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Subject not found");

        if (string.IsNullOrEmpty(updatedSubject.Name?.Trim()))
            throw new FormatException("Name can't not be null or empty");

        updatedSubject.Id = itemId;
        updatedSubject.AccountId = requesterUser.AccountId;
        updatedSubject.CreatorId = subjectCheck.CreatorId;
        updatedSubject.CreationDate = subjectCheck.CreationDate;
        updatedSubject.UpdateDate = DateTime.Now;
        updatedSubject.UpdaterId = requesterUser.UserId;

        return await _subjectRepository.UpdateAsync(updatedSubject);
    }
}
