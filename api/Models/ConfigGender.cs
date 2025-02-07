using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.Models 
{
    [Index("Title", IsUnique = true)]
    public class ConfigGender 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(256, ErrorMessage = "Title cannot exceed 256 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000, ErrorMessage = "Description cannot exceed 4000 characters.")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // [MaxLength(36)]
        // public string? InsertedById { get; set; }
        
        // [MaxLength(36)]
        // public string? UpdatedById { get; set; }
    }
}