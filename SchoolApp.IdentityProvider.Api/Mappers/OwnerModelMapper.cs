using SchoolApp.IdentityProvider.Api.Models.Users;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Api.Mappers;

public static class OwnerModelMapper
{
    public static Owner MapToOwner(this OwnerCreateModel model)
    {
        return new Owner()
        {
            Email = model.Email,
            Name = model.Name,
            DocumentId = model.DocumentId,
            IsMainOwner = model.IsMainOwner,
            Password = model.Password,
            ImageUrl = model.ImageUrl
        };
    }

    public static Owner MapToOwner(this OwnerUpdateModel model)
    {
        return new Owner()
        {
            Email = model.Email,
            Name = model.Name,
            DocumentId = model.DocumentId,
            IsMainOwner = model.IsMainOwner,
            ImageUrl = model.ImageUrl
        };
    }
}
