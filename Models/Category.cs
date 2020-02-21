using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductServices.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required( ErrorMessage = "Name must be at least 3 characters")]
        [StringLength( 50, MinimumLength = 3)]
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Brand> Commodities { get; set; }

    }
}