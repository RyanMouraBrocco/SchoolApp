using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolApp.Feed.Application.Domain.Entities;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.NoSql.Dtos;
using SchoolApp.Shared.Utils.MongoDb.Base;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Feed.NoSql.Repositories;

public class MessageRepository : BaseMainEntityRepository<MessageDto, Message>, IMessageRepository
{
    public MessageRepository(IOptions<MongoDbSettings> options, Func<MessageDto, Message> mapToDomain, Func<Message, MessageDto> mapToDto) : base(options, mapToDomain, mapToDto)
    {
    }
}
