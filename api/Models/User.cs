using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    [Index("Name", IsUnique = true)]
    [Index("Email", IsUnique = true)]
    public class User : IdentityUser
    {   
        [Key]
        [MaxLength(36)]
        public override string Id { get; set; } = string.Empty;

        public override string? UserName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(768, ErrorMessage = "Name cannot exceed 768 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public override string? Email { get; set; }

        public int? ConfigGenderId { get; set; }
        [ForeignKey("ConfigGenderId")]
        public ConfigGender? ConfigGender { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        public DateOnly BirthDate { get; set; }
        
        public bool IsActive { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // [MaxLength(36)]
        // public string? InsertedById { get; set; }
        
        // [MaxLength(36)]
        // public string? UpdatedById { get; set; }
    }
}