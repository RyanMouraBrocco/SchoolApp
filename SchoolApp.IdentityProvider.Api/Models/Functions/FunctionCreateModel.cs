using System.ComponentModel.DataAnnotations;

namespace SchoolApp.IdentityProvider.Api.Models.Functions;

public class FunctionModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}
