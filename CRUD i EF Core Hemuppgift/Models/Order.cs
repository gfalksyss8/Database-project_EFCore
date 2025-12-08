using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_i_EF_Core_Hemuppgift.Models
{
    public class Order
    {
        // Primary Key
        public int OrderID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required, MaxLength(100)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public decimal TotalAmount { get; set; }

        // Navigation Key
        [Required]
        public Customer? Customer { get; set; }

        // Foreign Key
        [Required]
        public int CustomerID { get; set; }

        [Required]
        public List<OrderRow> OrderRows { get; set; } = new();
    }
}
