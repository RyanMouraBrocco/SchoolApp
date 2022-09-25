using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolApp.Shared.Utils.MongoDb.Attributes;
using SchoolApp.Shared.Utils.MongoDb.Settings;

namespace SchoolApp.Shared.Utils.MongoDb.Base;

public class BaseRepository<TDto, TDomain> where TDto : class where TDomain : class
{
    protected readonly IMongoCollection<TDto> _collection;

    protected Func<TDto, TDomain> MapToDomain { get; set; }
    protected Func<TDomain, TDto> MapToDto { get; set; }

    public BaseRepository(IOptions<MongoDbSettings> options, Func<TDto, TDomain> mapToDomain, Func<TDomain, TDto> mapToDto)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var database = mongoClient.GetDatabase(options.Value.DatabaseName);
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
}
