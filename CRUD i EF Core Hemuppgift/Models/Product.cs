using System.ComponentModel.DataAnnotations;

namespace CRUD_i_EF_Core_Hemuppgift.Models
{
    public class Product
    {
        // Primary Key
        public int ProductID { get; set; }

        // Properties
        [Required, MaxLength(255)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        // Foreign keys
        public List<OrderRow> OrderRows { get; set; } = new();
    }
}
