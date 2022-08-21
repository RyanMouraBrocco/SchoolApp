namespace SchoolApp.IdentityProvider.Application.Domain.Users;

public class Manager : User
{
    public decimal Salary { get; set; }
    public DateTime HiringDate { get; set; }
    public int FunctionId { get; set; }
    public Function Function { get; set; }
}
