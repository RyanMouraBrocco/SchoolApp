namespace SchoolApp.IdentityProvider.Application.Domain.Users;

public class Function
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreateDate { get; set; }
}
