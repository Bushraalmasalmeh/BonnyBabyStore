﻿using System.ComponentModel.DataAnnotations;

namespace BonnyBabyStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
