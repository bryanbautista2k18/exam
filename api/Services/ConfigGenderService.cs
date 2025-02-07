using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;

namespace api.Services 
{
    public class ConfigGenderService : IConfigGenderService
    {
        private readonly IConfigGenderRepository _configGenderRepository;

        public ConfigGenderService(IConfigGenderRepository configGenderRepository)
        {
            _configGenderRepository = configGenderRepository ?? throw new ArgumentNullException(nameof(configGenderRepository));
        }

        public async Task<ResponseDTO<List<ConfigGenderDTO>>> CreateAsync(RequestConfigGenderDTO requestConfigGenderDTO)
        {
            return await _configGenderRepository.CreateAsync(requestConfigGenderDTO);
        } 

        public async Task<ResponseDTO<List<ConfigGenderDTO>>> GetAllAsync(string generalMessage)
        {
            return await _configGenderRepository.GetAllAsync(generalMessage);
        } 

        public async Task<ResponseDTO<ConfigGenderDTO?>> GetByIdAsync(int id)
        {
            return await _configGenderRepository.GetByIdAsync(id);
        } 

        public async Task<ResponseDTO<List<ConfigGenderDTO>?>> UpdateAsync(int id, RequestConfigGenderDTO requestConfigGenderDTO)
        {
            return await _configGenderRepository.UpdateAsync(id, requestConfigGenderDTO);
        } 

        public async Task<ResponseDTO<List<ConfigGenderDTO>?>> DeleteAsync(int id)
        {
            return await _configGenderRepository.DeleteAsync(id);
        } 
    }
}