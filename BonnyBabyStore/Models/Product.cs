using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonnyBabyStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string? Name { get; set;}
        public string? Description { get; set; }
       
        [Required(ErrorMessage = "Price is required")]
        [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

      public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
