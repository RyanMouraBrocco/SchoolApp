using System.Security.Principal;
using MongoDB.Bson;
using MongoDB.Driver;
using SchoolApp.Shared.Utils.Interfaces;
using SchoolApp.Shared.Utils.MongoDb.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Interfaces;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Shared.Utils.MongoDb.Base;

public class BaseCrudRepository<TDto, TDomain> : BaseRepository<TDto, TDomain>, ICrudRepository<TDomain, string> where TDomain : class where TDto : class, IIdentityEntity
{
    public BaseCrudRepository(MongoDbSettings options, Func<TDto, TDomain> mapToDomain, Func<TDomain, TDto> mapToDto) : base(options, mapToDomain, mapToDto)
    {
    }

    public virtual TDomain GetOneById(string id)
    {
        return MapToDomain(_collection.Find(x => x.Id == new ObjectId(id)).FirstOrDefault());
    }

    public virtual async Task<TDomain> InsertAsync(TDomain item)
    {
        var dto = MapToDto(item);
        dto.Id = ObjectId.GenerateNewId();
        await _collection.InsertOneAsync(dto);
        return MapToDomain(dto);
    }

    public virtual async Task<TDomain> UpdateAsync(TDomain item)
    {
        var dto = MapToDto(item);
        await _collection.ReplaceOneAsync(x => x.Id == dto.Id, dto);
        return item;
    }

    public virtual async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == new ObjectId(id));
    }
}
