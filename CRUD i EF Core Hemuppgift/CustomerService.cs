using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using CRUD_i_EF_Core_Hemuppgift.Models;
using System.Threading.Tasks;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace CRUD_i_EF_Core_Hemuppgift
{
    public class CustomerService
    {
        // The database .json file in bin/*
        private static readonly string FileName = Path.Combine(AppContext.BaseDirectory, "Customer.json");

        // Formatting options
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        // READ
        public async Task<List<Customer>> LoadAllAsync()
        {
            if (!File.Exists(FileName))
            {
                // Get empty list if file is not found/doesn't exist
                return new List<Customer>();
            }

            var json = await File.ReadAllTextAsync(FileName);
            var customers = JsonSerializer.Deserialize<List<Customer>>(json, _options);

            foreach (var customer in customers)
            {
                if (!string.IsNullOrEmpty(customer.Email))
                {
                    customer.Email = EncryptionHelper.Decrypt(customer.Email);
                }
            }

            // fallback incase of null pointer
            return customers ?? new List<Customer>();
        }

        // WRITE
        private async Task SaveAsync(List<Customer> customers)
        {
            var toSave = customers.Select(c => new Customer
            {
                CustomerID = c.CustomerID,
                Name = c.Name,
                City = c.City,
                Email = string.IsNullOrEmpty(c.Email)
                    ? c.Email
                    : EncryptionHelper.Encrypt(c.Email),
                Phone = c.Phone
            });

            var json = JsonSerializer.Serialize(toSave, _options);
            await File.WriteAllTextAsync(FileName, json);
        }

        public async Task AddAsync(Customer customer)
        {
            var customers = await LoadAllAsync();
            // Auto-incrementing of IDs to get the next 
            customer.CustomerID = customers.Any() ? customers.Max(c => c.CustomerID) + 1 : 1;

            customers.Add(customer);
            await SaveAsync(customers);
        }

        public async Task UpdateAsync(int customerID, string? Name, string? Email, string? City, string? Phone)
        {
            var customers = await LoadAllAsync();

            // Find the customer ID for the inputted int ID
            var customer = customers.FirstOrDefault(c => c.CustomerID == customerID);
            if (customer==null)
            {
                Console.WriteLine("Customer not found");
                return;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                customer.Name = Name;
            }

            if (!string.IsNullOrEmpty(Email))
            {
                customer.Email = Email;
            }

            if (!string.IsNullOrEmpty(City))
            {
                customer.City = City;
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                customer.Phone = Phone;
            }

            await SaveAsync(customers);
        }

        public async Task DeleteAsync(int CustomerID)
        {
            var customers = await LoadAllAsync();

            // Find the customer from ID
            var customer = customers.FirstOrDefault(c => c.CustomerID==CustomerID);
            if (customer==null)
            {
                Console.WriteLine("Customer not found");
                return;
            }

            customers.Remove(customer);

            await SaveAsync(customers);
        }

        public async Task AddOrderToCustomer(int customerID, Order order)
        {
            var customers = await LoadAllAsync();
            // Find the customer ID for the inputted int ID
            var customer = customers.FirstOrDefault(c => c.CustomerID == customerID);
            if (customer == null)
            {
                Console.WriteLine("Customer not found");
                return;
            }

            customer.Orders.Add(order);

            await SaveAsync(customers);
        }
    }
}
