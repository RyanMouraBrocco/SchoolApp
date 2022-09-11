using SchoolApp.Classroom.Application.Domain.Entities.Subjects;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Subjects;
using SchoolApp.Classroom.Sql.Mappers.Subjects;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public class SubjectRepository : BaseMainEntityRepository<SubjectDto, Subject, SchoolAppClassroomContext>, ISubjectRepository
{
    public SubjectRepository(SchoolAppClassroomContext context) : base(context, SubjectMapper.MapToDomain, SubjectMapper.MapToDto)
    {
    }
}

