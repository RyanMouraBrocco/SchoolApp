using SchoolApp.Activity.Api.Models.Activities;

namespace SchoolApp.Activity.Api.Mappers;

public static class ActivityModelMapper
{
    public static Application.Domain.Entities.Activities.Activity MapToActivity(this ActivityCreateModel model)
    {
        return new Application.Domain.Entities.Activities.Activity()
        {
            Name = model.Name,
            Description = model.Description,
            ClassroomId = model.ClassroomId,
            Closed = model.Closed
        };
    }

    public static Application.Domain.Entities.Activities.Activity MapToActivity(this ActivityUpdateModel model)
    {
        return new Application.Domain.Entities.Activities.Activity()
        {
            Name = model.Name,
            Description = model.Description,
            ClassroomId = model.ClassroomId
        };
    }
}
