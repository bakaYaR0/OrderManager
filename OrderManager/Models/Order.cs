using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManager.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int ProviderId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "nvarchar(max)")]
        public string Number { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "datetime2(7)")]
        public DateTime Date { get; set; }
    }
}
