﻿using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Category
{
    public class CategoryCreate
    {
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!&]+$", ErrorMessage = "Category name must contain at least one letter and cannot be only numbers.")]
        public string Name { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
