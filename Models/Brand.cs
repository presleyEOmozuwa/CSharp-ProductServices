using System.ComponentModel.DataAnnotations;

namespace ProductServices.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        public int CommodityId { get; set; }
        public int CategoryId { get; set; }
        public Commodity commodity { get; set; }
        public Category category { get; set; }
    }
}