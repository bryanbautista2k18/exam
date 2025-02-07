using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using api.DTOs;

namespace api.Interfaces 
{
    public interface IConfigGenderService
    {
        Task<ResponseDTO<List<ConfigGenderDTO>>> CreateAsync(RequestConfigGenderDTO requestConfigGenderDTO);
        Task<ResponseDTO<List<ConfigGenderDTO>>> GetAllAsync(string generalMessage);
        Task<ResponseDTO<ConfigGenderDTO?>> GetByIdAsync(int id);
        Task<ResponseDTO<List<ConfigGenderDTO>?>> UpdateAsync(int id, RequestConfigGenderDTO requestConfigGenderDTO);
        Task<ResponseDTO<List<ConfigGenderDTO>?>> DeleteAsync(int id);
    }
}