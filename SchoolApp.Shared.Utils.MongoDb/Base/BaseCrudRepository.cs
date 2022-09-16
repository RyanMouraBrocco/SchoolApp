using System.Security.Principal;
using MongoDB.Bson;
using MongoDB.Driver;
using SchoolApp.Shared.Utils.Interfaces;
using SchoolApp.Shared.Utils.MongoDb.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Interfaces;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Shared.Utils.MongoDb.Base;

public class BaseCrudRepository<TDto, TDomain> : ICrudRepository<TDomain, string> where TDomain : class where TDto : class, IIdentityEntity
{
    private readonly IMongoCollection<TDto> _collection;

    protected Func<TDto, TDomain> MapToDomain { get; set; }
    protected Func<TDomain, TDto> MapToDto { get; set; }

    public BaseCrudRepository(MongoDbSettings options, Func<TDto, TDomain> mapToDomain, Func<TDomain, TDto> mapToDto)
    {
        var mongoClient = new MongoClient(options.ConnectionString);
        var database = mongoClient.GetDatabase(options.DatabaseName);
        _collection = database.GetCollection<TDto>(GetCollectionName(typeof(TDto)));
        MapToDomain = mapToDomain;
        MapToDto = mapToDto;
    }

    private string GetCollectionName(Type documentType)
    {
        return ((CollectionNameAttribute)documentType.GetCustomAttributes(
                typeof(CollectionNameAttribute),
                true)
            .FirstOrDefault())?.Name;
    }

    public TDomain GetOneById(string id)
    {
        return MapToDomain(_collection.Find(x => x.Id == new ObjectId(id)).FirstOrDefault());
    }

    public async Task<TDomain> InsertAsync(TDomain item)
    {
        var dto = MapToDto(item);
        dto.Id = ObjectId.GenerateNewId();
        await _collection.InsertOneAsync(dto);
        return MapToDomain(dto);
    }

    public async Task<TDomain> UpdateAsync(TDomain item)
    {
        var dto = MapToDto(item);
        await _collection.ReplaceOneAsync(x => x.Id == dto.Id, dto);
        return item;
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == new ObjectId(id));
    }
}
