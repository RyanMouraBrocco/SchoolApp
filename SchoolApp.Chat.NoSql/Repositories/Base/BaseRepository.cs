using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SchoolApp.Chat.NoSql.Attributes;
using SchoolApp.Chat.NoSql.Settings;

namespace SchoolApp.Chat.NoSql.Repositories.Base;

public class BaseRepository<TDto, TDomain>
{
    public Func<TDto, TDomain> MapToDomain { get; set; }
    public Func<TDomain, TDto> MapToDto { get; set; }

    protected readonly CollectionReference _collection;

    public BaseRepository(IOptions<FirebaseSettings> options, Func<TDto, TDomain> mapToDomain, Func<TDomain, TDto> mapToDto)
    {
        var builder = new FirestoreClientBuilder();
        builder.JsonCredentials = options.Value.PrivateKeyJson;
        var firebaseDb = FirestoreDb.Create(options.Value.ProjectId, builder.Build());
        _collection = firebaseDb.Collection(GetCollectionName(typeof(TDto)));

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
