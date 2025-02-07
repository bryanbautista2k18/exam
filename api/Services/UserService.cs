using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;

namespace api.Services 
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<ResponseDTO<List<UserDTO>>> CreateAsync(RequestUserDTO requestUserDTO)
        {
            return await _userRepository.CreateAsync(requestUserDTO);
        } 

        public async Task<ResponseDTO<List<IData>>> GetAllAsync(string generalMessage)
        {
            return await _userRepository.GetAllAsync(generalMessage);
        } 

        public async Task<ResponseDTO<UserDTO?>> GetByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        } 

        public async Task<ResponseDTO<List<UserDTO>?>> UpdateAsync(string id, RequestUserDTO requestUserDTO)
        {
            return await _userRepository.UpdateAsync(id, requestUserDTO);
        } 

        public async Task<ResponseDTO<List<UserDTO>?>> DeleteAsync(string id)
        {
            return await _userRepository.DeleteAsync(id);
        } 
    }
}