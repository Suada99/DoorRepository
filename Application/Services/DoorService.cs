using Application.Services.Interfaces;
using Core.Entities;
using Core.Repositories;

namespace Application.Services
{
    public class DoorService : IDoorService
    {
        private readonly IRepository<User> _userService;
        public DoorService(IRepository<User> userService)
        {
            _userService = userService;
        }

        public async Task<CommandResult<User>> EnterOfficeAsync(User user)
        {
            try
            {
                user.InOffice = true;
                await _userService.UpdateAsync(user);
            }
            catch (Exception e)
            {
                return new CommandResult<User>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        Code = "500",
                        Description = e.Message,
                        HttpCode = System.Net.HttpStatusCode.InternalServerError
                    }
                };
            }
            return new CommandResult<User>
            {
                Success = true,
                Data = user
            };
        }

        public async Task<CommandResult<User>> LeaveOfficeAsync(User user)
        {
            try
            {
                if (!user.InOffice)
                {
                    return new CommandResult<User>
                    {
                        Success = false,
                        CommandError = new CommandError
                        {
                            Code = "404",
                            HttpCode = System.Net.HttpStatusCode.NotFound,
                            Description = "There is no user found in office"
                        }
                    };
                }
                user.InOffice = false;
                await _userService.UpdateAsync(user);
            }
            catch (Exception e)
            {
                return new CommandResult<User>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        Code = "500",
                        Description = e.Message,
                        HttpCode = System.Net.HttpStatusCode.InternalServerError
                    }
                };
            }
            return new CommandResult<User>
            {
                Success = true,
                Data = user
            };
        }
    }
}
