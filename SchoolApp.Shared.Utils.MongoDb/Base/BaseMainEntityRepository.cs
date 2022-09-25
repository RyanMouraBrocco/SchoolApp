using System.Security.Principal;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SchoolApp.Shared.Utils.MongoDb.Interfaces;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Shared.Utils.MongoDb.Base;

public class BaseMainEntityRepository<TDto, TDomain> : BaseCrudRepository<TDto, TDomain> where TDto : class, IIdentityEntity, IAccountEntity, ISoftDeleteEntity where TDomain : class
{
    public BaseMainEntityRepository(IOptions<MongoDbSettings> options, Func<TDto, TDomain> mapToDomain, Func<TDomain, TDto> mapToDto) : base(options, mapToDomain, mapToDto)
    {
    }

    public override async Task DeleteAsync(string id)
    {
        var updateQuery = Builders<TDto>.Update.Set(x => x.Deleted, true);
        await _collection.UpdateOneAsync(x => x.Id == new ObjectId(id), updateQuery);
    }

    public override TDomain GetOneById(string id)
    {
        return MapToDomain(_collection.Find(x => x.Id == new ObjectId(id) && !x.Deleted).FirstOrDefault());
    }

    public override async Task<TDomain> InsertAsync(TDomain item)
    {
        var dto = MapToDto(item);
        dto.Id = ObjectId.GenerateNewId();
        dto.Deleted = false;
        await _collection.InsertOneAsync(dto);
        return MapToDomain(dto);
    }

    public IList<TDomain> GetAll(int accountId, int top, int skip)
    {
        return _collection.Find(x => x.AccountId == accountId && !x.Deleted).Skip(skip).Limit(top).ToList().Select(x => MapToDomain(x)).ToList();
    }
}
