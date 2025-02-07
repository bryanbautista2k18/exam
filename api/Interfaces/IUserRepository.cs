using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using api.DTOs;

namespace api.Interfaces 
{
    public interface IUserRepository
    {
        Task<ResponseDTO<List<UserDTO>>> CreateAsync(RequestUserDTO requestUserDTO);
        Task<ResponseDTO<List<IData>>> GetAllAsync(string generalMessage);
        Task<ResponseDTO<UserDTO?>> GetByIdAsync(string id);
        Task<ResponseDTO<List<UserDTO>?>> UpdateAsync(string id, RequestUserDTO requestUserDTO);
        Task<ResponseDTO<List<UserDTO>?>> DeleteAsync(string id);
    }
}