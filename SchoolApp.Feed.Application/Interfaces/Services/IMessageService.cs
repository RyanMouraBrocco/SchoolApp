using SchoolApp.Shared.Utils.Interfaces;
using SchoolApp.Feed.Application.Domain.Entities;

namespace SchoolApp.Feed.Application.Interfaces.Services;

public interface IMessageService : ICrudService<Message, string>
{
}
