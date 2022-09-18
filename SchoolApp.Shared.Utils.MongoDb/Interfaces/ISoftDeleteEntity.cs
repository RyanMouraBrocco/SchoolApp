namespace SchoolApp.Shared.Utils.MongoDb.Interfaces;

public interface ISoftDeleteEntity
{
    bool Deleted { get; set; }
}
