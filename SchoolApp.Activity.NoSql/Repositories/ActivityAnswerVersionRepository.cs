using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.NoSql.Dtos.Answers;
using SchoolApp.Activity.NoSql.Mappers.Activity;
using SchoolApp.Shared.Utils.MongoDb.Base;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Activity.NoSql.Repositories;

public class ActivityAnswerVersionRepository : BaseCrudRepository<ActivityAnswerVersionDto, ActivityAnswerVersion>, IActivityAnswerVersionRepository
{
    public ActivityAnswerVersionRepository(MongoDbSettings options) : base(options, ActivityAnswerVersionMapper.MapToDomain, ActivityAnswerVersionMapper.MapToDto)
    {
    }
}
