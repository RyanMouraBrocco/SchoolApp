namespace SchoolApp.IdentityProvider.Api.Models.Users;

public class OwnerModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ImageUrl { get; set; }
    public string DocumentId { get; set; }
    public bool IsMainOwner { get; set; }
}
