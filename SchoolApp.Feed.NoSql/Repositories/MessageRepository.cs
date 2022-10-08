using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolApp.Feed.Application.Domain.Entities;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.NoSql.Dtos;
using SchoolApp.Feed.NoSql.Mappers;
using SchoolApp.Shared.Utils.MongoDb.Base;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Feed.NoSql.Repositories;

public class MessageRepository : BaseMainEntityRepository<MessageDto, Message>, IMessageRepository
{
    public MessageRepository(IOptions<MongoDbSettings> options) : base(options, MessageMapper.MapToDomain, MessageMapper.MapToDto)
    {
    }

    public IList<Message> GetAllMainMessages(int accountId, int top, int skip)
    {
        return _collection.Find(x => x.AccountId == accountId && !x.Deleted && x.MessageId == null).Skip(skip).Limit(top).ToList().Select(x => MapToDomain(x)).ToList();
    }
}
