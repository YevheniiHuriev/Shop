using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shop.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "CreateDate is required")]
        public DateTime CreateDate { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        // Navigation property
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
