using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Interfaces;

namespace SchoolApp.Activity.NoSql.Dtos.Answers;

[CollectionName("ActivityAnswer")]
public class ActivityAnswerDto : IIdentityEntity, IAccountEntity, ISoftDeleteEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    public ObjectId ActivityId { get; set; }
    public int StudentId { get; set; }
    public int AccountId { get; set; }
    public DateTime CreationDate { get; set; }
    public ActivityAnswerVersionDto LastReview { get; set; }
    public bool Deleted { get; set; }
}
