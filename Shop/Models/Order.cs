using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Order
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "OrderDate is required")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "TotalAmount ...")]
        [Range(0.01, 100000.00, ErrorMessage = "min: 0.01, max: 100000.00")]
        public decimal TotalAmount { get; set; } = decimal.Zero;

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = string.Empty;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
