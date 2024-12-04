using System.ComponentModel.DataAnnotations;

namespace Moodeng.Models
{
    public class Product
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string Picture { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

    }
}