using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.NoSql.Dtos.Activities;
using SchoolApp.Activity.NoSql.Mappers.Activity;
using SchoolApp.Shared.Utils.MongoDb.Base;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Activity.NoSql.Repositories;

public class ActivityRepository : BaseMainEntityRepository<ActivityDto, Application.Domain.Entities.Activities.Activity>, IActivityRepository
{
    public ActivityRepository(IOptions<MongoDbSettings> options) : base(options, ActivityMapper.MapToDomain, ActivityMapper.MapToDto)
    {
    }

    public IList<Application.Domain.Entities.Activities.Activity> GetAllByClassroomsIds(IEnumerable<int> classroomsIds)
    {
        return _collection.Find(x => classroomsIds.Contains(x.ClassroomId) && !x.Deleted).ToList().Select(x => MapToDomain(x)).ToList();
    }
}
