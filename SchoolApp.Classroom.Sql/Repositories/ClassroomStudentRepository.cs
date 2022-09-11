using Microsoft.EntityFrameworkCore;
using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Classrooms;
using SchoolApp.Classroom.Sql.Mappers.Classrooms;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public class ClassroomStudentRepository : BaseCrudRepository<ClassroomStudentDto, ClassroomStudent, SchoolAppClassroomContext>, IClassroomStudentRepository
{
    public ClassroomStudentRepository(SchoolAppClassroomContext context) : base(context, ClassroomStudentMapper.MapToDomain, ClassroomStudentMapper.MapToDto)
    {
    }

    public async Task DeleteAllByClassroomIdAsync(int classroomId)
    {
        var allStudents = _dbSet.AsNoTracking().Where(x => x.ClassroomId == classroomId).ToList();
        _dbSet.RemoveRange(allStudents);
        await _context.SaveChangesAsync();
    }
}
