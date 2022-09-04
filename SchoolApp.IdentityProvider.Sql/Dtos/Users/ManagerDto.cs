using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.IdentityProvider.Sql.Dtos.Functions;

namespace SchoolApp.IdentityProvider.Sql.Dtos.Users;

[Table("Manager")]
public class ManagerDto : UserDto
{
    public decimal Salary { get; set; }
    public DateTime HiringDate { get; set; }
    public int FunctionId { get; set; }
    [ForeignKey("FunctionId")]
    public FunctionDto Function { get; set; }
}
