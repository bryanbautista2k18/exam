using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bogus;
using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Mappers;
using api.Models;

namespace api.Repositories 
{
    public class UserRepository : IUserRepository 
    {
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly Faker _faker;
        private readonly AppDbContext _dBContext;

        public UserRepository(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext dBContext
        )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)); 
            _dBContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
            _faker = new Faker();
        }

        bool HasNonAlphanumericCharacter(string password)
        {
            foreach (var c in password)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        bool HasDigit(string password)
        {
            foreach (var c in password)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<ResponseDTO<List<UserDTO>>> CreateAsync(RequestUserDTO requestUserDTO)
        {
            using var transaction = _dBContext.Database.BeginTransaction();

            try 
            {
                var errors = new List<Error>();

                if (await _userManager.Users.AnyAsync(u => u.Name == requestUserDTO.Name))
                {
                    errors.Add(new Error("name", "Name already exists."));
                }
                if (await _userManager.Users.AnyAsync(u => u.Email == requestUserDTO.Email))
                {
                    errors.Add(new Error("email", "Email already exists."));
                }
                if (!await _dBContext.ConfigGenders.AnyAsync(cg => cg.Id == requestUserDTO.ConfigGenderId))
                {
                    errors.Add(new Error("configGenderId", "Config Gender is invalid."));
                }

                if (errors.Count > 0)
                {
                    return ResponseDTO<List<UserDTO>>.Failure(
                        message: new Message("danger", "Failed to create a user."),
                        errors: errors
                    );
                }

                var userModel = requestUserDTO.ToUser();

                // Generate fake username.
                userModel.UserName = _faker.Internet.UserName();

                // Generate fake password.
                var password = _faker.Internet.Password(length: 12);
                while (!HasNonAlphanumericCharacter(password) || !HasDigit(password))
                {
                    password = _faker.Internet.Password(length: 12);
                }

                var result = await _userManager.CreateAsync(userModel, password);
                if (result.Succeeded)
                {
                    await transaction.CommitAsync();
                    return ResponseDTO<List<UserDTO>>.Success(
                        message: new Message("success", "Successfully created a user.")
                    );
                }

                await transaction.RollbackAsync();
                errors = result.Errors.GroupBy(e => e.Code).Select(g => new Error(g.Key.ToString(), string.Join(',', g.Select(e => e.Description)))).ToList();
                
                return ResponseDTO<List<UserDTO>>.Failure(
                    message: new Message("danger", "Failed to create a user."),
                    errors: errors
                );
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ResponseDTO<List<IData>>> GetAllAsync(string generalMessage)
        {
            var query = _httpContextAccessor.HttpContext?.Request.Query;
            if (query == null)
            {
                throw new InvalidOperationException("HttpContext or Request.Query is null.");
            }

            string searchKey = "";
            int page = 1;
            int limit = 10;
            if (query.ContainsKey("search_key"))
            {
                searchKey = (query["search_key"].ToString() ?? "");
            }
            if (query.ContainsKey("page") && int.TryParse(query["page"], out var parsedPage))
            {
                page = parsedPage;
            }
            if (query.ContainsKey("limit") && int.TryParse(query["limit"], out var parsedLimit))
            {
                limit = parsedLimit;
            }
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            var configGenders = new List<AliasedConfigGenderDTO>();
            if (query.ContainsKey("genders"))
            {
                if (query["genders"].ToString().ToLower() == "all")
                {
                    configGenders = await _dBContext.ConfigGenders
                        .AsNoTracking()
                        .Where(cg => cg.IsActive)
                        .OrderBy(cg => cg.Title)
                        .Select(cg => cg.ToAliasedConfigGenderDTO())
                        .ToListAsync();
                }
            }
          
            var users = await _userManager.Users
                .AsNoTracking()
                .Include(u => u.ConfigGender)
                .Where(u => 
                    string.IsNullOrWhiteSpace(searchKey) 
                    ? (u.IsActive) 
                    : (u.Name.Contains(searchKey) && u.IsActive)
                )
                .OrderBy(u => u.Name)
                .Select(u => u.ToUserDTO())
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var message = (
                !string.IsNullOrWhiteSpace(generalMessage)
                ? new Message("success", generalMessage)
                : null
            );

            var data = new Dictionary<string, List<IData>>
            {
                { "users", users.Cast<IData>().ToList() }
            };
            
            if (configGenders.Count > 0) 
            {
                data.Add("configGenders", configGenders.Cast<IData>().ToList());
            }

            return ResponseDTO<List<IData>>.Success(
                message: message,
                data: data
            );
        }

        public async Task<ResponseDTO<UserDTO?>> GetByIdAsync(string id)
        {
            var userModel = await _dBContext.Users
                .Include(u => u.ConfigGender)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userModel == null || !userModel.IsActive)
            {
                return ResponseDTO<UserDTO?>.Failure(
                    message: new Message("danger", "Record not found.")
                );
            }
            
            return ResponseDTO<UserDTO?>.Success(
                data: new Dictionary<string, UserDTO?>
                {
                    { "user", userModel.ToUserDTO() }
                }
            );
        }

        public async Task<ResponseDTO<List<UserDTO>?>> UpdateAsync(string id, RequestUserDTO requestUserDTO)
        {
            using var transaction = _dBContext.Database.BeginTransaction();

            try 
            {
                var userModel = await _userManager.FindByIdAsync(id);

                if (userModel == null || !userModel.IsActive)
                {
                    _httpContextAccessor.HttpContext!.Response.StatusCode = 404;
                    return ResponseDTO<List<UserDTO>?>.Failure(
                        message: new Message("danger", "Record not found.")
                    );
                }

                var errors = new List<Error>();

                // Check for existing Name.
                if (await _userManager.Users.AnyAsync(u => u.Id != id && u.Name == requestUserDTO.Name))
                {
                    errors.Add(new Error("name", "Name already exists."));
                }
                // Check for existing Email.
                if (await _userManager.Users.AnyAsync(u => u.Id != id && u.Email == requestUserDTO.Email))
                {
                    errors.Add(new Error("email", "Email already exists."));
                }
                if (!await _dBContext.ConfigGenders.AnyAsync(cg => cg.Id == requestUserDTO.ConfigGenderId))
                {
                    errors.Add(new Error("configGenderId", "Config Gender is invalid."));
                }

                if (errors.Count > 0)
                {
                    _httpContextAccessor.HttpContext!.Response.StatusCode = 422;
                    return ResponseDTO<List<UserDTO>?>.Failure(
                        message: new Message("danger", "Failed to update the user."),
                        errors: errors
                    );
                }

                userModel.Name = requestUserDTO.Name.Trim();
                userModel.Email = requestUserDTO.Email.Trim();
                userModel.ConfigGenderId = requestUserDTO.ConfigGenderId;
                userModel.BirthDate = requestUserDTO.BirthDate;

                var entry = _dBContext.Entry(userModel);
                if (entry.State == EntityState.Unchanged) 
                {
                    _httpContextAccessor.HttpContext!.Response.StatusCode = 400;
                    return ResponseDTO<List<UserDTO>?>.Failure(
                        message: new Message("danger", "No changes were made."),
                        errors: errors
                    );
                }

                var result = await _userManager.UpdateAsync(userModel);
                if (result.Succeeded)
                {
                    await transaction.CommitAsync();
                    return ResponseDTO<List<UserDTO>?>.Success(
                        message: new Message("success", "Successfully updated the user.")
                    );
                }

                await transaction.RollbackAsync();
                errors = result.Errors.GroupBy(e => e.Code).Select(g => new Error(g.Key.ToString(), string.Join(',', g.Select(e => e.Description)))).ToList();
                
                return ResponseDTO<List<UserDTO>?>.Failure(
                    message: new Message("danger", "Failed to update the user."),
                    errors: errors
                );
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        } 

        public async Task<ResponseDTO<List<UserDTO>?>> DeleteAsync(string id)
        {
            using var transaction = _dBContext.Database.BeginTransaction();

            try 
            {
                var userModel = await _userManager.FindByIdAsync(id);

                if (userModel == null || !userModel.IsActive)
                {
                    return ResponseDTO<List<UserDTO>?>.Failure(
                        message: new Message("danger", "Record not found.")
                    );
                }

                userModel.IsActive = false;
                // var result = await _userManager.DeleteAsync(user);

                var result = await _userManager.UpdateAsync(userModel);
                if (result.Succeeded)
                {
                    await transaction.CommitAsync();
                    return ResponseDTO<List<UserDTO>?>.Success(
                        message: new Message("info", "Successfully deleted the user.")
                    );
                }

                await transaction.RollbackAsync();
                var errors = result.Errors.GroupBy(e => e.Code).Select(g => new Error(g.Key.ToString(), string.Join(',', g.Select(e => e.Description)))).ToList();
                
                return ResponseDTO<List<UserDTO>?>.Failure(
                    message: new Message("danger", "Failed to delete the user."),
                    errors: errors
                );
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        } 
    }
}