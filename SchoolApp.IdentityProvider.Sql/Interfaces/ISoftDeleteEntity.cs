namespace SchoolApp.IdentityProvider.Sql.Interfaces;

public interface ISoftDeleteEntity
{
    public bool Deleted { get; set; }
}
