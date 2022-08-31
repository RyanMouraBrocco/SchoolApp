namespace SchoolApp.IdentityProvider.Application.Domain.Functions;

public class Function
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
}
