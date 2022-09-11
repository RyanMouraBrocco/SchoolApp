using SchoolApp.Classroom.Sql.Dtos.Classrooms;
using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;

namespace SchoolApp.Classroom.Sql.Mappers.Classrooms;

public static class ClassroomMapper
{
    public static Application.Domain.Entities.Classrooms.Classroom MapToDomain(ClassroomDto dto)
    {
        return new Application.Domain.Entities.Classrooms.Classroom()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            RoomNumber = dto.RoomNumber,
            SubjectId = dto.SubjectId,
            TeacherId = dto.TeacherId,
            UpdaterId = dto.UpdaterId,
            UpdateDate = dto.UpdateDate
        };
    }

    public static ClassroomDto MapToDto(Application.Domain.Entities.Classrooms.Classroom domain)
    {
        return new ClassroomDto()
        {
            Id = domain.Id,
            AccountId = domain.AccountId,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            RoomNumber = domain.RoomNumber,
            SubjectId = domain.SubjectId,
            TeacherId = domain.TeacherId,
            UpdaterId = domain.UpdaterId,
            UpdateDate = domain.UpdateDate
        };
    }
}
