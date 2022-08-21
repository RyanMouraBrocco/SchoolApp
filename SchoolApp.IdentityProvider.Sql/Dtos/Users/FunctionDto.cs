namespace SchoolApp.IdentityProvider.Sql.Dtos.Users;

public class FunctionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreateDate { get; set; }
}
