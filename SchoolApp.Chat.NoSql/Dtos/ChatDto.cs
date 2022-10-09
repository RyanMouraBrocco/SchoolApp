using Google.Cloud.Firestore;
using SchoolApp.Chat.NoSql.Attributes;

namespace SchoolApp.Chat.NoSql.Dtos;

[CollectionName("Chat")]
public class ChatDto
{
    [FirestoreDocumentId]
    public string Id { get; set; }
    public int AccountId { get; set; }
    public int User1Id { get; set; }
    public int User1Type { get; set; }
    public int User2Id { get; set; }
    public int User2Type { get; set; }
    public DateTime CreationDate { get; set; }
}
