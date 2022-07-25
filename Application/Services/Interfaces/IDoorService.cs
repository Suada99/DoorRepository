using Core.Entities;

namespace Application.Services.Interfaces
{
    public interface IDoorService
    {
        Task<CommandResult<User>> EnterOfficeAsync(User user);
        Task<CommandResult<User>> LeaveOfficeAsync(User user);    }
}
