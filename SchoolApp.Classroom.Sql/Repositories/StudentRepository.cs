using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Classroom.Sql.Mappers.Students;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public class StudentRepository : BaseMainEntityRepository<StudentDto, Student, SchoolAppClassroomContext>, IStudentRepository
{
    public StudentRepository(SchoolAppClassroomContext context) : base(context, StudentMapper.MapToDomain, StudentMapper.MapToDto)
    {
    }

    public IList<Student> GetAllByOwnerId(int ownerId, int top, int skip)
    {
        return _dbSet.AsNoTracking()
                     .Where(x => x.Owners.Any(x => x.OwnerId == ownerId) && !x.Deleted)
                     .Skip(skip)
                     .Take(top)
                     .Select(x => MapToDomain(x))
                     .ToList();
    }

    public Student GetOneByIdAndOwnerId(int id, int ownerId)
    {
        return _dbSet.AsNoTracking()
                     .Where(x => x.Id == id && x.Owners.Any(x => x.OwnerId == ownerId) && !x.Deleted)
                     .Select(x => MapToDomain(x))
                     .FirstOrDefault();
    }

    public IList<Student> GetAllByTeacherId(int teacherId, int top, int skip)
    {
        return _dbSet.AsNoTracking()
                     .Where(x => x.Classrooms.Any(x => x.Classroom.TeacherId == teacherId) && !x.Deleted)
                     .Skip(skip)
                     .Take(top)
                     .Select(x => MapToDomain(x))
                     .ToList();
    }

    public Student GetOneByIdAndTeacherId(int id, int teacherId)
    {
        return _dbSet.AsNoTracking()
                     .Where(x => x.Id == id && x.Classrooms.Any(x => x.Classroom.TeacherId == teacherId) && !x.Deleted)
                     .Select(x => MapToDomain(x))
                     .FirstOrDefault();
    }
}
