using System;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Chat.Application.Domain.Entities;

public class Chat
{
    public string Id { get; set; }
    public int AccountId { get; set; }
    public int User1Id { get; set; }
    public UserTypeEnum User1Type { get; set; }
    public int User2Id { get; set; }
    public UserTypeEnum User2Type { get; set; }
    public DateTime CreationDate { get; set; }
}
