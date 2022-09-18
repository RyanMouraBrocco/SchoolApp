using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Interfaces;

namespace SchoolApp.Activity.NoSql.Dtos.Answers;

[CollectionName("ActivityAnswerVersion")]
public class ActivityAnswerVersionDto : IIdentityEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    public ObjectId ActivityAnswerId { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
}
