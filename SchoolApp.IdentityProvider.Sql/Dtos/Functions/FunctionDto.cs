using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.IdentityProvider.Sql.Dtos.Functions;

[Table("Function")]
public class FunctionDto : IIdentityEntity, IAccountEntity, ISoftDeleteEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool Deleted { get; set; }
}
