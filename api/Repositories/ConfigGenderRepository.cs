using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Mappers;
using api.Models;

namespace api.Repositories 
{
    public class ConfigGenderRepository : IConfigGenderRepository 
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dBContext;

        public ConfigGenderRepository(
            IHttpContextAccessor httpContextAccessor,
            AppDbContext dBContext
        )
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)); 
            _dBContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
        }

        public async Task<ResponseDTO<List<ConfigGenderDTO>>> CreateAsync(RequestConfigGenderDTO requestConfigGenderDTO)
        {
            using var transaction = _dBContext.Database.BeginTransaction();

            try 
            {
                var errors = new List<Error>();

                // Check for existing title.
                if (await _dBContext.ConfigGenders.AnyAsync(cg => cg.Title == requestConfigGenderDTO.Title))
                {
                    errors.Add(new Error("title", "Title already exists."));
                }
               
                if (errors.Count > 0)
                {
                    return ResponseDTO<List<ConfigGenderDTO>>.Failure(
                        message: new Message("danger", "Failed to create a gender."),
                        errors: errors
                    );
                }

                var configGenderModel = requestConfigGenderDTO.ToConfigGender();

                await _dBContext.ConfigGenders.AddAsync(configGenderModel);
                await _dBContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResponseDTO<List<ConfigGenderDTO>>.Success(
                    message: new Message("success", "Successfully created a gender.") 
                );
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ResponseDTO<List<ConfigGenderDTO>>> GetAllAsync(string generalMessage)
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

            var configGenders = await _dBContext.ConfigGenders
                .AsNoTracking()
                .Where(cg => 
                    string.IsNullOrWhiteSpace(searchKey) 
                    ? (cg.IsActive) 
                    : (cg.Title.Contains(searchKey) && cg.IsActive)
                )
                .OrderBy(cg => cg.Title)
                .Select(cg => cg.ToConfigGenderDTO())
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return ResponseDTO<List<ConfigGenderDTO>>.Success(
                message: (
                    !string.IsNullOrWhiteSpace(generalMessage)
                    ? new Message("success", generalMessage) 
                    : null
                ),
                data:  new Dictionary<string, List<ConfigGenderDTO>>
                {
                    { "configGenders", configGenders },
                }
            );
        }

        public async Task<ResponseDTO<ConfigGenderDTO?>> GetByIdAsync(int id)
        {
            var configGenderModel = await _dBContext.ConfigGenders.FindAsync(id);

            if (configGenderModel == null || !configGenderModel.IsActive)
            {
                return ResponseDTO<ConfigGenderDTO?>.Failure(
                    message: new Message("danger", "Record not found.")
                );
            }

            return ResponseDTO<ConfigGenderDTO?>.Success(
                data: new Dictionary<string, ConfigGenderDTO?>
                {
                    { "configGender", configGenderModel.ToConfigGenderDTO() }
                }
            );
        }

        public async Task<ResponseDTO<List<ConfigGenderDTO>?>> UpdateAsync(int id, RequestConfigGenderDTO requestConfigGenderDTO)
        {
            using var transaction = _dBContext.Database.BeginTransaction();

            try 
            {
                var configGenderModel = await _dBContext.ConfigGenders.FindAsync(id);

                if (configGenderModel == null || !configGenderModel.IsActive)
                {
                    _httpContextAccessor.HttpContext!.Response.StatusCode = 404;
                    return ResponseDTO<List<ConfigGenderDTO>?>.Failure(
                        message: new Message("danger", "Record not found.")
                    );
                }

                // Check for existing title.
                var errors = new List<Error>();
                if (await _dBContext.ConfigGenders.AnyAsync(cg => cg.Id != id && cg.Title == requestConfigGenderDTO.Title))
                {
                    errors.Add(new Error("title", "Title already exists."));
                }
                if (errors.Count > 0)
                {
                    _httpContextAccessor.HttpContext!.Response.StatusCode = 422;
                    return ResponseDTO<List<ConfigGenderDTO>?>.Failure(
                        message: new Message("danger", "Failed to update the gender."),
                        errors: errors
                    );
                }

                configGenderModel.Title = requestConfigGenderDTO.Title.Trim();
                configGenderModel.Description = requestConfigGenderDTO.Description.Trim();

                var entry = _dBContext.Entry(configGenderModel);
                if (entry.State == EntityState.Unchanged) 
                {
                    _httpContextAccessor.HttpContext!.Response.StatusCode = 400;
                    return ResponseDTO<List<ConfigGenderDTO>?>.Failure(
                        message: new Message("danger", "No changes were made."),
                        errors: errors
                    );
                }

                await _dBContext.SaveChangesAsync();
                await transaction.CommitAsync();
            
                return ResponseDTO<List<ConfigGenderDTO>?>.Success(
                    message: new Message("success", "Successfully updated the gender.")
                );
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        } 

        public async Task<ResponseDTO<List<ConfigGenderDTO>?>> DeleteAsync(int id)
        {
            using var transaction = _dBContext.Database.BeginTransaction();

            try 
            {
                var configGenderModel = await _dBContext.ConfigGenders.FindAsync(id);

                if (configGenderModel == null || !configGenderModel.IsActive)
                {
                    return ResponseDTO<List<ConfigGenderDTO>?>.Failure(
                        message: new Message("danger", "Record not found.")
                    );
                }

                configGenderModel.IsActive = false;
                // _dBContext.ConfigGenders.Remove(configGenderModel);

                await _dBContext.SaveChangesAsync();
                await transaction.CommitAsync();
            
                return ResponseDTO<List<ConfigGenderDTO>?>.Success(
                    message: new Message("info", "Successfully deleted the gender.")
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