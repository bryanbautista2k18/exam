using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Mappers 
{
    public static class UserMapper
    {
        public static User ToUser(this RequestUserDTO createUserDTO)
        {
            return new User 
            {
                Name = createUserDTO.Name.Trim(),
                Email = createUserDTO.Email.Trim(),
                ConfigGenderId = createUserDTO.ConfigGenderId,
                BirthDate = createUserDTO.BirthDate,
                IsActive = true
            };
        }

        public static UserDTO ToUserDTO(this User user)
        {
            return new UserDTO 
            {
                Id = user.Id,
                Name = user.Name,
                Email = (user.Email ?? string.Empty),
                ConfigGender = user.ConfigGender,
                BirthDate = user.BirthDate,
                Age = GetAge(user.BirthDate),
                IsActive = user.IsActive
            };
        }

        static int GetAge(DateOnly birthDate)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now); 

            int years = currentDate.Year - birthDate.Year;

            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
            {
                years--;
            }

            return years;
        }
    }
}