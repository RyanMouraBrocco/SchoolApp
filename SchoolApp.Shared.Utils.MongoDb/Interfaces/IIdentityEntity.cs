using MongoDB.Bson;

namespace SchoolApp.Shared.Utils.MongoDb.Interfaces;

public interface IIdentityEntity
{
    ObjectId Id { get; set; }
}
