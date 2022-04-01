using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManager.Models
{
    public class Provider
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "nvarchar(max)")]
        public string Name { get; set; }
    }
}
