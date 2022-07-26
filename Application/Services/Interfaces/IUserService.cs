﻿using Application.Models.DTOs;
using Core.Entities.Enum;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<CommandResult<List<UserDto>>> GetAllUsers(TagStatus? tagStatus);
        Task<CommandResult<bool>> UpdateUserTag(Guid userId, TagStatus tagStatus);
    }
}