using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductServices.Models
{
    public class Commodity
    {
        [Key]
        public int CommodityId { get; set; }
        
        [Required( ErrorMessage = "Name must be at least 3 characters")]
        [StringLength( 100, MinimumLength = 3 )]
        public string Name { get; set; }
        
        
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        [Range( 1, Int32.MaxValue )]
        public int Quantity { get; set; }
        
        
        [Required( ErrorMessage = "Description must be at least 5 characters")]
        [StringLength( 255, MinimumLength = 5, ErrorMessage = "You must provide a Description" )]
        public string Description { get; set; }
        [Required]
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Brand> Categories { get; set; }
    }
}