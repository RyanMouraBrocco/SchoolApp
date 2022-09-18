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
    public ActivityAnswerRepository(MongoDbSettings options) : base(options, ActivityAnswerMapper.MapToDomain, ActivityAnswerMapper.MapToDto)
    {
    }

    public async Task SetLastReview(string id, ActivityAnswerVersion version)
    {
        var updateQuery = Builders<ActivityAnswerDto>.Update.Set(x => x.LastReview, ActivityAnswerVersionMapper.MapToDto(version));
        await _collection.UpdateOneAsync(x => x.Id == new MongoDB.Bson.ObjectId(id), updateQuery);
    }
}
