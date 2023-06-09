﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    [Table("Category")]
    public class Category
    {
        public Category()
        {
            Name = string.Empty;
            Icon = "https://picsum.photos/50";
            Subcategories = new HashSet<Subcategory>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }
        public string Icon { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; }
    }
}
