namespace SchoolApp.IdentityProvider.Application.Domain.Entities.Accounts;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}
