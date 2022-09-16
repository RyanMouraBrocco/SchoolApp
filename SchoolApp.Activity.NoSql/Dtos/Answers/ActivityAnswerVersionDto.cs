using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Attributes;

namespace SchoolApp.Activity.NoSql.Dtos.Answers;

[CollectionName("ActivityAnswerVersion")]
public class ActivityAnswerVersionDto
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonId]
    public ObjectId ActivityAnswerId { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
}
