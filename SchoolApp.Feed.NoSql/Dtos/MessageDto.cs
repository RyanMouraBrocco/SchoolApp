using MongoDB.Bson;
using SchoolApp.Shared.Utils.MongoDb.Interfaces;

namespace SchoolApp.Feed.NoSql.Dtos;

public class MessageDto : IIdentityEntity, IAccountEntity, ISoftDeleteEntity
{
    public ObjectId Id { get; set; }
    public int AccountId { get; set; }
    public ObjectId? MessageId { get; set; }
    public string Text { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool Deleted { get; set; }
}
