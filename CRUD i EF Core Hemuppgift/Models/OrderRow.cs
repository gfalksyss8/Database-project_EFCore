using System.ComponentModel.DataAnnotations;

namespace CRUD_i_EF_Core_Hemuppgift.Models
{
    public class OrderRow
    {
        // Primary Key
        public int OrderRowID { get; set; }

        // Properties
        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        // Foreign keys
        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        // Nav keys
        public Order? Order { get; set; }

        public Product? Product { get; set; }
    }
}
