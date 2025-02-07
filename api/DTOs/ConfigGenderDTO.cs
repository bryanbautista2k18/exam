using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;

namespace api.DTOs
{
    public class ConfigGenderDTO 
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }

    public class RequestConfigGenderDTO
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }

    public class AliasedConfigGenderDTO : IData
    {
        public string Text { get; init; } = string.Empty;
        public int Value { get; init; }
    }
}