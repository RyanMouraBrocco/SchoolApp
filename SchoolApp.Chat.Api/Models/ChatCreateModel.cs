using System.ComponentModel.DataAnnotations;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Chat.Api.Models;

public class ChatCreateModel
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public UserTypeEnum UserType { get; set; }
}
