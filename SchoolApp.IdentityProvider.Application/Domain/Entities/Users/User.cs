namespace SchoolApp.IdentityProvider.Application.Domain.Users;

public class User
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ImageUrl { get; set; }
    public string DocumentId { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
}
