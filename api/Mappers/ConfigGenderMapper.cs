using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Mappers 
{
    public static class ConfigGenderMapper
    {
        public static ConfigGender ToConfigGender(this RequestConfigGenderDTO requestConfigGenderDTO)
        {
            return new ConfigGender 
            {
                Title = requestConfigGenderDTO.Title.Trim(),
                Description = requestConfigGenderDTO.Description.Trim(),
                IsActive = true
            };
        }

        public static ConfigGenderDTO ToConfigGenderDTO(this ConfigGender configGender)
        {
            return new ConfigGenderDTO 
            {
                Id = configGender.Id,
                Title = configGender.Title,
                Description = (configGender.Description ?? string.Empty),
                IsActive = configGender.IsActive
            };
        }

        public static AliasedConfigGenderDTO ToAliasedConfigGenderDTO(this ConfigGender configGender)
        {
            return new AliasedConfigGenderDTO
            {
                Text = configGender.Title,
                Value = configGender.Id
            };
        }
    }
}