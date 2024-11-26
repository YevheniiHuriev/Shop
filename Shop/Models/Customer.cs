using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Humanizer;

namespace Shop.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name is required ... min: 2, max: 20")]
        public string Name { get; set; } = string.Empty;
        // Navigation property
        [JsonIgnore] // To fix an error in serializing nested objects
        public ICollection<Order>? Orders { get; set; }
    }
}