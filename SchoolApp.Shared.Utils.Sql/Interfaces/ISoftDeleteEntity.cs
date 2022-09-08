namespace SchoolApp.Shared.Utils.Sql.Interfaces;

public interface ISoftDeleteEntity
{
    public bool Deleted { get; set; }
}
