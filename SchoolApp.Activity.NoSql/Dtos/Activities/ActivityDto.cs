using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Interfaces;

namespace SchoolApp.Activity.NoSql.Dtos.Activities;

[CollectionName("Activity")]
public class ActivityDto : IIdentityEntity, IAccountEntity, ISoftDeleteEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ClassroomId { get; set; }
    public DateTime? CloseDate { get; set; }
    public bool Closed { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool Deleted { get; set; }
}
