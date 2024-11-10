using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_Repository.Request
{
    public class UpdateSilverJewerlyRequest
    {
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$",
    ErrorMessage = "Each word in SilverJewelryName must start with a capital letter and can include letters, spaces, and digits.")]
        public string SilverJewelryName { get; set; } = null!;

        [Required]
        public DateTime? CreatedDate { get; set; }

        [Required]
        public string? SilverJewelryDescription { get; set; }

        [Required]
        public decimal? MetalWeight { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        public decimal? Price { get; set; }

        [Required]
        [Range(1900, int.MaxValue, ErrorMessage = "ProductionYear must be greater than or equal to 1900.")]
        public int? ProductionYear { get; set; }

        [Required]
        public string? CategoryId { get; set; }
    }
}
