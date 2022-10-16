using Microsoft.EntityFrameworkCore;
using SchoolApp.Classroom.Sql.Dtos.Classrooms;
using SchoolApp.Classroom.Sql.Dtos.Grades;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Classroom.Sql.Dtos.Subjects;
using SchoolApp.Shared.Utils.Sql.Contexts;

namespace SchoolApp.Classroom.Sql.Context;

public class SchoolAppClassroomContext : SchoolAppContext
{
    public DbSet<StudentDto> Student { get; set; }
    public DbSet<OwnerStudentDto> OwnerStudent { get; set; }
    public DbSet<ClassroomDto> Classroom { get; set; }
    public DbSet<ClassroomStudentDto> ClassroomStudent { get; set; }
    public DbSet<ActivityAnswerGradeDto> ActivityAnswerGrade { get; set; }
    public DbSet<ClassroomStudentGradeDto> ClassroomStudentGrade { get; set; }
    public DbSet<SubjectDto> Subject { get; set; }

    public SchoolAppClassroomContext(DbContextOptions<SchoolAppClassroomContext> options) : base(options)
    {

    }
}
