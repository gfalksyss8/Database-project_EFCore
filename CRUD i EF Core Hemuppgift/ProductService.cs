using CRUD_i_EF_Core_Hemuppgift.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace CRUD_i_EF_Core_Hemuppgift
{
    public class ProductService
    {
        // The database .json file in bin/*
        private static readonly string FileName = Path.Combine(AppContext.BaseDirectory, "Product.json");

        // Formatting options
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        // READ
        public async Task<List<Product>> LoadAllAsync()
        {
            if (!File.Exists(FileName))
            {
                // Get empty list if file is not found/doesn't exist
                return new List<Product>();
            }

            var json = await File.ReadAllTextAsync(FileName);
            var products = JsonSerializer.Deserialize<List<Product>>(json, _options);

            // fallback incase of null pointer
            return products ?? new List<Product>();
        }

        // WRITE
        private async Task SaveAsync(List<Product> products)
        {
            var toSave = products.Select(p => new Product
            {
                ProductID = p.ProductID,
                Name = p.Name,
                Price = p.Price,
            });

            var json = JsonSerializer.Serialize(toSave, _options);
            await File.WriteAllTextAsync(FileName, json);
        }

        public async Task AddAsync(Product product)
        {
            var products = await LoadAllAsync();
            // Auto-incrementing of IDs to get the next 
            product.ProductID = products.Any() ? products.Max(c => c.ProductID) + 1 : 1;

            products.Add(product);
            await SaveAsync(products);
        }

        public async Task UpdateAsync(int productID, string? name, string? price)
        {
            var products = await LoadAllAsync();

            // Find the product ID for the inputted int ID
            var product = products.FirstOrDefault(p => p.ProductID == productID);
            if (product == null)
            {
                Console.WriteLine("Product not found");
                return;
            }

            if (!string.IsNullOrEmpty(name))
            {
                product.Name = name;
            }

            if (!string.IsNullOrEmpty(price) && decimal.TryParse(price, out decimal parsedPrice))
            {
                
                product.Price = parsedPrice;
            }
            else
            {
                Console.WriteLine("Invalid price input. Price not updated.");
            }

                await SaveAsync(products);
        }

        public async Task DeleteAsync(int ProductID)
        {
            var products = await LoadAllAsync();

            // Find the customer from ID
            var product = products.FirstOrDefault(p => p.ProductID == ProductID);
            if (product == null)
            {
                Console.WriteLine("Product not found");
                return;
            }

            products.Remove(product);

            await SaveAsync(products);
        }
    }
}
