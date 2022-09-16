using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Attributes;

namespace SchoolApp.Activity.NoSql.Dtos.Answers;

[CollectionName("ActivityAnswer")]
public class ActivityAnswerDto
{
    [BsonId]
    public ObjectId Id { get; set; }
    public ObjectId ActivityId { get; set; }
    public int StudentId { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
    public ActivityAnswerVersionDto LastReview { get; set; }
    public bool Deleted { get; set; }
}
