namespace SchoolApp.Feed.Application.Interfaces.Repositories;

public interface IMessageAllowedPermissionRepository<TDto>
{
    void Send(TDto message);
}
