using Microsoft.Extensions.Options;
using SchoolApp.Chat.Application.Interfaces.Repositories;
using SchoolApp.Chat.NoSql.Dtos;
using SchoolApp.Chat.NoSql.Mappers;
using SchoolApp.Chat.NoSql.Repositories.Base;
using SchoolApp.Chat.NoSql.Settings;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Chat.NoSql.Repositories;

public class ChatRepository : BaseRepository<ChatDto, Application.Domain.Entities.Chat>, IChatRepository
{
    public ChatRepository(IOptions<FirebaseSettings> options) : base(options, ChatMapper.MapToDomain, ChatMapper.MapToDto)
    {
    }

    public async Task DeleteAsync(string id)
    {
        var newDocument = _collection.Document(id);
        await newDocument.DeleteAsync();
    }

    public IList<Application.Domain.Entities.Chat> GetAllByUserId(int userId, UserTypeEnum type, int top, int skip)
    {
        var dtos = _collection.WhereEqualTo("User1Id", userId)
                              .WhereEqualTo("User1Type", (int)type)
                              .GetSnapshotAsync().Result.Select(x => x.ConvertTo<ChatDto>()).ToList();

        dtos.AddRange(_collection.WhereEqualTo("User2Id", userId)
                                 .WhereEqualTo("User2Type", (int)type)
                                 .GetSnapshotAsync().Result.Select(x => x.ConvertTo<ChatDto>()).ToList());

        return dtos.Select(x => MapToDomain(x)).ToList();
    }

    public Application.Domain.Entities.Chat GetOneById(string id)
    {
        var dto = _collection.WhereEqualTo("Id", id).GetSnapshotAsync().Result.FirstOrDefault()?.ConvertTo<ChatDto>();
        return MapToDomain(dto);
    }

    public Application.Domain.Entities.Chat GetOneByUsers(int userId, UserTypeEnum type, int user2Id, UserTypeEnum user2Type)
    {
        var dto = _collection.WhereEqualTo("User1Id", userId)
                             .WhereEqualTo("User1Type", (int)type)
                             .WhereEqualTo("User2Id", user2Id)
                             .WhereEqualTo("User2Type", (int)user2Type)
                             .GetSnapshotAsync().Result.FirstOrDefault()?.ConvertTo<ChatDto>();

        if (dto == null)
        {
            dto = _collection.WhereEqualTo("User1Id", user2Id)
                             .WhereEqualTo("User1Type", (int)user2Type)
                             .WhereEqualTo("User2Id", userId)
                             .WhereEqualTo("User2Type", (int)type)
                             .GetSnapshotAsync().Result.FirstOrDefault()?.ConvertTo<ChatDto>();
        }

        return MapToDomain(dto);
    }

    public async Task<Application.Domain.Entities.Chat> InsertAsync(Application.Domain.Entities.Chat item)
    {
        var dto = MapToDto(item);
        var newDocument = _collection.Document();
        var writeResult = await newDocument.CreateAsync(dto);
        return MapToDomain((await newDocument.GetSnapshotAsync())?.ConvertTo<ChatDto>());
    }

    public async Task<Application.Domain.Entities.Chat> UpdateAsync(Application.Domain.Entities.Chat item)
    {
        var dto = MapToDto(item);
        var updateDocument = _collection.Document(dto.Id);
        var writeResult = await updateDocument.SetAsync(dto);
        return MapToDomain((await updateDocument.GetSnapshotAsync())?.ConvertTo<ChatDto>());
    }
}
