using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_i_EF_Core_Hemuppgift.Models
{
    public class ProductSale
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }

    }
}
