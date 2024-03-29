﻿using BLL.AuthLogic.DTOS;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BLL.AuthLogic.Commands
{
    public class RegisterCommand : IRequest<UserDTO>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
        public string? Name { get; set; }
    }
}
