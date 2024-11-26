using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shop.Models
{
    public class OrderProduct
    {
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [JsonIgnore] // To fix an error in serializing nested objects
        public Order? Order { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public  Product? Product { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater then zero")]
        public int Quantity { get; set; }
    }
}
