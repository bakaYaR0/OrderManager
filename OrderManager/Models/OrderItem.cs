using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManager.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "nvarchar(max)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "nvarchar(max)")]
        public string Unit { get; set; }
    }
}
