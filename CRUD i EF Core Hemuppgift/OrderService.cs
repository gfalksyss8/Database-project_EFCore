using CRUD_i_EF_Core_Hemuppgift.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace CRUD_i_EF_Core_Hemuppgift
{
    public class OrderService
    {
        // The database .json file in bin/*
        private static readonly string FileName = Path.Combine(AppContext.BaseDirectory, "Order.json");

        // Formatting options
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        // READ
        public async Task<List<Order>> LoadAllAsync()
        {
            if (!File.Exists(FileName))
            {
                // Get empty list if file is not found/doesn't exist
                return new List<Order>();
            }

            var json = await File.ReadAllTextAsync(FileName);
            var orders = JsonSerializer.Deserialize<List<Order>>(json, _options);

            // fallback incase of null pointer
            return orders ?? new List<Order>();
        }

        // WRITE
        private async Task SaveAsync(List<Order> orders)
        {
            var toSave = orders.Select(o => new Order
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.Status,
                OrderRows = o.OrderRows,
                TotalAmount = o.TotalAmount,
            });

            var json = JsonSerializer.Serialize(toSave, _options);
            await File.WriteAllTextAsync(FileName, json);
        }

        public async Task AddAsync(Order order)
        {
            var orders = await LoadAllAsync();
            // Auto-incrementing of IDs to get the next 
            order.OrderID = orders.Any() ? orders.Max(o => o.OrderID) + 1 : 1;

            orders.Add(order);
            await SaveAsync(orders);
        }

        public async Task UpdateAsync(int orderID, string? Status, int changeDays, List<OrderRow> orderRows)
        {
            var orders = await LoadAllAsync();

            // Find the order ID for the inputted int ID
            var order = orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order == null)
            {
                Console.WriteLine("Order not found");
                return;
            }

            if (!string.IsNullOrEmpty(Status))
            {
                order.Status = Status;
            }

            order.OrderDate.AddDays(changeDays);

            if (orderRows.Any())
            {
                order.OrderRows = orderRows;
            }

            await SaveAsync(orders);
        }

        public async Task DeleteAsync(int orderID)
        {
            var orders = await LoadAllAsync();

            // Find the customer from ID
            var order = orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order == null)
            {
                Console.WriteLine("Order not found");
                return;
            }

            orders.Remove(order);

            await SaveAsync(orders);
        }
    }
}
