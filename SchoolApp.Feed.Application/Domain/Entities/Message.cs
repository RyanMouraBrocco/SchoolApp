namespace SchoolApp.Feed.Application.Domain.Entities;

public class Message
{
    public string Id { get; set; }
    public int AccountId { get; set; }
    public string MessageId { get; set; }
    public string Text { get; set; }
    public int CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime? UpdateDate { get; set; }
}
