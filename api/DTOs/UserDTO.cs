using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.DTOs
{
    public class UserDTO : IData
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public ConfigGender? ConfigGender { get; init; }
        public DateOnly BirthDate { get; init; }
        public int Age { get; init; }
        public bool IsActive { get; init; }
    }

    public class RequestUserDTO
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public int ConfigGenderId { get; init; }
        public DateOnly BirthDate { get; init; }
    }
}