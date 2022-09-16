namespace SchoolApp.Shared.Utils.MongoDb.Attributes;

public class CollectionNameAttribute : Attribute
{
    public string Name { get; private set; }

    public CollectionNameAttribute(string name)
    {
        Name = name;
    }
}
