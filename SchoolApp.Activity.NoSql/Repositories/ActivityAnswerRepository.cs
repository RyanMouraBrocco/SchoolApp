using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.NoSql.Dtos.Answers;
using SchoolApp.Activity.NoSql.Mappers.Activity;
using SchoolApp.Shared.Utils.MongoDb.Base;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Activity.NoSql.Repositories;

public class ActivityAnswerRepository : BaseMainEntityRepository<ActivityAnswerDto, ActivityAnswer>, IActivityAnswerRepository
{
    public ActivityAnswerRepository(IOptions<MongoDbSettings> options) : base(options, ActivityAnswerMapper.MapToDomain, ActivityAnswerMapper.MapToDto)
    {
    }

    public IList<ActivityAnswer> GetAllByActitvityId(string activityId, int top, int skip)
    {
        return _collection.Find(x => x.ActivityId == new MongoDB.Bson.ObjectId(activityId)).Skip(skip).Limit(top).ToList().Select(x => MapToDomain(x)).ToList();
    }

    public IList<ActivityAnswer> GetAllByActitvityIdAndStudentsIds(string activityId, IEnumerable<int> studentsIds)
    {
        return _collection.Find(x => x.ActivityId == new MongoDB.Bson.ObjectId(activityId) && studentsIds.Contains(x.StudentId)).ToList().Select(x => MapToDomain(x)).ToList();
    }

    public async Task SetLastReview(string id, ActivityAnswerVersion version)
    {
        var updateQuery = Builders<ActivityAnswerDto>.Update.Set(x => x.LastReview, ActivityAnswerVersionMapper.MapToDto(version));
        await _collection.UpdateOneAsync(x => x.Id == new MongoDB.Bson.ObjectId(id), updateQuery);
    }
}
