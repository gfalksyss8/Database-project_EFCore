using System.ComponentModel.DataAnnotations;

namespace CRUD_i_EF_Core_Hemuppgift.Models

{
    public class Customer
    {
        // Primary Key
        public int CustomerID { get; set; }

        // Properties
        [Required, MaxLength(255)]
        public string Name { get; set; } = null;

        [Required, MaxLength(255)]
        public string Email { get; set; } = null;

        [Required, MaxLength(255)]
        public string City { get; set; } = null;

        [MaxLength(100)]
        public string Phone { get; set; }

        // Foreign Key
        // Customer can have many orders (0..N)
        public List<Order> Orders { get; set; } = new();
    }
}