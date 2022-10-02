using MongoDB.Bson;
using SchoolApp.Shared.Utils.MongoDb.Interfaces;

namespace SchoolApp.Feed.NoSql.Dtos;

public class MessageDto : IIdentityEntity, IAccountEntity, ISoftDeleteEntity
{
    public ObjectId Id { get; set; }
    public int AccountId { get; set; }
    public bool Deleted { get; set; }
}
