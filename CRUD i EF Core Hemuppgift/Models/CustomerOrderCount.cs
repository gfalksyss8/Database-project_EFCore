using Microsoft.EntityFrameworkCore;

namespace CRUD_i_EF_Core_Hemuppgift.Models
{
    [Keyless]
    public class CustomerOrderCount
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public int numberOfOrders { get; set; }
    }
}
