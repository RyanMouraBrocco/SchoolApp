namespace SchoolApp.IdentityProvider.Application.Domain.Users;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ImageUrl { get; set; }
    public string DocumentId { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreateDate { get; set; }
}
