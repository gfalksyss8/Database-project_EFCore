using CRUD_i_EF_Core_Hemuppgift;
using CRUD_i_EF_Core_Hemuppgift.Migrations;
using CRUD_i_EF_Core_Hemuppgift.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

Console.WriteLine("DB: " + Path.Combine(AppContext.BaseDirectory, "shop.db"));

// Ensure DB + migrations + seeding
using (var db = new ShopContext())
{
    // Migrate Async: Creates the database
    // if it does not exist
    await db.Database.MigrateAsync();

    // Seeding into database manually
    // Only run if category is empty
    // Seeding for customers
    if (!await db.Customers.AnyAsync())
    {
        // TODO: Local customer seeding
        db.Customers.AddRange(
            new Customer { Name = "Erik", Email = "erik.stattebratt@gmail.com", City = "Malmö"},
            new Customer { Name= "Adam", Email = "adam.fillibang@gmail.com", City = "Stockholm"} 
            );
        await db.SaveChangesAsync();
        Console.WriteLine("DB Customers Seeded");
    }

    // Seeding for Products
    // TODO: Seed products!
    if (!await db.Products.AnyAsync())
    {
        db.Products.AddRange(
            new Product { Name = "Hammer", Price = 49M },
            new Product { Name = "Screwdriver", Price = 29M },
            new Product { Name = "Wrench", Price = 39M },
            new Product { Name = "Pencil", Price = 9M }
            );
        await db.SaveChangesAsync();
        Console.WriteLine("DB Products Seeded");
    }

    // Seeding for Orders
    if (!await db.Orders.AnyAsync())
    {
        var orderRows1 = new List<OrderRow>
        {
            new OrderRow { ProductID = 1, Quantity = 5, UnitPrice = 49M },
            new OrderRow { ProductID = 2, Quantity = 2, UnitPrice = 29M }
        };
        var orderRows2 = new List<OrderRow>
        {
            new OrderRow { ProductID = 3, Quantity = 3, UnitPrice = 39M},
            new OrderRow { ProductID = 4, Quantity = 4, UnitPrice = 9M}
        };

        db.Orders.AddRange(
            new Order { OrderDate = DateTime.Now.AddDays(-14), Status = "Pending", CustomerID = 1, OrderRows = orderRows1, TotalAmount = orderRows1.Sum(o => o.UnitPrice * o.Quantity) },
            new Order { OrderDate = DateTime.Now.AddDays(-30), Status = "Pending", CustomerID = 2, OrderRows = orderRows2, TotalAmount = orderRows2.Sum(o => o.UnitPrice * o.Quantity) }
            );
        await db.SaveChangesAsync();
        Console.WriteLine("DB Orders Seeded");
    }

    // CRUD: Create, Read, Update, Delete
    while (true)
    {
        Console.WriteLine("\nUsage: <entity> <command> (Example: customers list)");
        Console.WriteLine("Entities: customers | orders | orderRows | products | clearall | exit");
        Console.WriteLine("Commands: list | add | delete <id> | edit <id> | summary (orders) | ordercount (customers) | sales (products)");
        Console.Write("> ");
        var choice = Console.ReadLine()?.Trim() ?? string.Empty;

        // If user input is empty, do nothing
        if (string.IsNullOrEmpty(choice))
        {
            continue;
        }

        // When user input is "exit" ...
        if (choice.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            break; // ... Jump out of loop to exit program
        }

        // If user input is "clearall" 
        if (choice.Equals("clearall", StringComparison.OrdinalIgnoreCase)) {
            await ClearAll();
            continue;
        }

        // Split string choice into array by space
        // Example "customers edit 2" --> ["customers", "edit", "2"]
        var parts = choice.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var entChoice = parts[0].ToLowerInvariant();
        var cmdChoice = parts[1].ToLowerInvariant();

        // Switch for CRUD
        switch (entChoice)
        {
            case "customers":
                switch (cmdChoice)
                {
                    case "list":
                        await ListAsync(entChoice);
                        break;
                    case "add":
                        await AddAsync(entChoice);
                        break;
                    case "delete":
                        if (parts.Length < 3 || !int.TryParse(parts[2], out var idDelete))
                        {
                            Console.WriteLine("Usage: Delete <id>");
                            break;
                        }
                        await DeleteAsync(entChoice, idDelete);
                        break;
                    case "edit":
                        if (parts.Length < 3 || !int.TryParse(parts[2], out var idEdit))
                        {
                            Console.WriteLine("Usage: Delete <id>");
                            break;
                        }
                        await EditAsync(entChoice, idEdit);
                        break;
                    case "ordercount":
                        await OrderCountAsync();
                        break;
                }
                break;

            case "orders":
                switch (cmdChoice)
                {
                    case "list":
                        await ListAsync(entChoice);
                        break;
                    case "add":
                        await AddAsync(entChoice);
                        break;
                    case "delete":
                        if (parts.Length < 3 || !int.TryParse(parts[2], out var idDelete))
                        {
                            Console.WriteLine("Usage: Delete <id>");
                            break;
                        }
                        await DeleteAsync(entChoice, idDelete);
                        break;
                    case "edit":
                        if (parts.Length < 3 || !int.TryParse(parts[2], out var idEdit))
                        {
                            Console.WriteLine("Usage: Delete <id>");
                            break;
                        }
                        await EditAsync(entChoice, idEdit);
                        break;
                    case "summary":
                        await ListOrderSummary();
                        break;
                }
                break;

            case "orderrows":
                switch (cmdChoice)
                {
                    case "list":
                        await ListAsync(entChoice);
                        break;
                    case "add":
                        await AddAsync(entChoice);
                        break;
                    case "delete":
                        if (parts.Length < 3 || !int.TryParse(parts[2], out var idDelete))
                        {
                            Console.WriteLine("Usage: Delete <id>");
                            break;
                        }
                        await DeleteAsync(entChoice, idDelete);
                        break;
                    case "edit":
                        if (parts.Length < 3 || !int.TryParse(parts[2], out var idEdit))
                        {
                            Console.WriteLine("Usage: Delete <id>");
                            break;
                        }
                        await EditAsync(entChoice, idEdit);
                        break;
                }
                break;

            case "products":
                switch (cmdChoice)
                {
                    case "list":
                        await ListAsync(entChoice);
                        break;
                    case "add":
                        await AddAsync(entChoice);
                        break;
                    case "delete":
                        if (parts.Length < 3 || !int.TryParse(parts[2], out var idDelete))
                        {
                            Console.WriteLine("Usage: Delete <id>");
                            break;
                        }
                        await DeleteAsync(entChoice, idDelete);
                        break;
                    case "edit":
                        if (parts.Length < 3 || !int.TryParse(parts[2], out var idEdit))
                        {
                            Console.WriteLine("Usage: Delete <id>");
                            break;
                        }
                        await EditAsync(entChoice, idEdit);
                        break;
                    case "sales":
                        await ProductSalesAsync(); 
                        break;
                }
                break;
        }
    }
}

// METHODS

// Bring user's chosen entity (STR) to method, use to choose case in switch
// If applicable, bring ID (INT) for targeting specific element

// LIST - Lists the chosen entities 
static async Task ListAsync(string entity)
{
    using var db = new ShopContext();

    switch (entity)
    {
        // Chosen entity = customers
        case "customers":
            var customers = await db.Customers
                                    .AsNoTracking()
                                    .Include(customers => customers.Orders)
                                    .OrderBy(customers => customers.CustomerID)
                                    .ToListAsync();

            Console.WriteLine("ID | Name | Email | City | Total Orders");

            foreach (var customer in customers)
            {
                Console.WriteLine(customer.CustomerID + " | " + customer.Name + " | " + customer.Email + " | " + customer.City + " | " + customer.Orders?.Count);
                Console.Write(customer.Name + "'s orders: ");
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine(order.OrderID + " | " + order.TotalAmount);
                }
                Console.WriteLine("\n");
            }

            break;

        // Chosen entity = orders
        case "orders":
            var orders = await db.Orders
                                    .AsNoTracking()
                                    .Include(order => order.Customer)
                                    .OrderBy(order => order.OrderID)
                                    .ToListAsync();

            Console.WriteLine("ID | Customer | Order Date | Status | Total Amount | Rows in Order");

            foreach (var order in orders)
            {
                Console.WriteLine(order.OrderID + " | " + order.Customer?.Name + " | " + order.OrderDate + " | " + order.Status + " | " + order.TotalAmount + " | " +  order.OrderRows?.Count);
            }

            break;

        // Chosen entity = orderRows
        case "orderrows":
            var orderrows = await db.OrderRows
                                    .AsNoTracking()
                                    .Include(orderr => orderr.Product)
                                    .OrderBy(orderr => orderr.OrderID)
                                    .ToListAsync();
            Console.WriteLine("ID | Exists in OrderID: | Product | Quantity | Unit Price");
            foreach (var orderrow in orderrows)
            {
                Console.WriteLine(orderrow.OrderRowID + " | " + orderrow.OrderID + " | " + orderrow.Product?.Name + " | " + orderrow.Quantity + " | " + orderrow.UnitPrice);
            }
            break;

        // Chosen entity = products
        case "products":
            var products = await db.Products
                                    .AsNoTracking()
                                    .OrderBy(p => p.ProductID)
                                    .ToListAsync();
            Console.WriteLine("ID | Name | Price");
            foreach (var product in products)
            {
                Console.WriteLine(product.ProductID + " | " + product.Name + " | " + product.Price);
            }
            break;

    }
}


// ADD - Add 1 entry for the chosen entity
// TODO: IMPLEMENT PRINT FOR METHODS: (OrderRows, Products)
static async Task AddAsync(string entity)
{
    using var db = new ShopContext();

    switch (entity)
    {
        // Chosen entity = customers
        case "customers":
            // Name
            Console.WriteLine("Enter customer's name:");
            var customerName = Console.ReadLine()?.Trim() ?? string.Empty;
            // Validate Customer.Name
            if (string.IsNullOrEmpty(customerName) || customerName.Length > 255)
            {
                Console.WriteLine("Name is required, and cannot exceed 255 characters");
                return;
            }

            // Email
            Console.WriteLine("Enter customer email: ");
            var customerEmail = Console.ReadLine()?.Trim() ?? string.Empty;
            // Validate Customer.Email
            if (string.IsNullOrEmpty(customerEmail) || customerEmail.Length > 255)
            {
                Console.WriteLine("Email is required, and cannot exceed 255 characters");
                return;
            }
            else if (db.Customers.Any(c => c.Email == customerEmail))
            {
                Console.WriteLine("Email already exists");
                return;
            }

            // City
            Console.WriteLine("Enter city: ");
            var customerCity = Console.ReadLine()?.Trim() ?? string.Empty;
            // Validate Customer.City
            if (string.IsNullOrEmpty(customerCity) || customerCity.Length > 255)
            {
                Console.WriteLine("City is required, and cannot exceed 255 characters");
                return;
            }

            // Add all inputs to DB
            db.Customers.Add(new Customer { Name = customerName, Email = customerEmail, City = customerCity});
            // Try to save DB
            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Customer successfully added");
            }
            catch (DbUpdateException failedSavingException)
            {
                Console.WriteLine("DB Saving error: " + failedSavingException.Message);
            }

            break;

        // Chosen entity = orders
        case "orders":
            Console.Write("Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out var customerID) ||
                !db.Customers.Any(c => c.CustomerID == customerID))
            {
                Console.WriteLine("Error: Unknown Customer ID");
            }

            var order = new Order
            {
                CustomerID = customerID,

            };

            var orderRows = new List<OrderRow>();

            while (true) {
                Console.WriteLine("Add order row? y/n");
                var answer = Console.ReadLine()?.Trim() ?? string.Empty;
                if (answer != "y") break;

                var products = await db.Products.AsNoTracking()
                                                .OrderBy(o => o.ProductID)
                                                .ToListAsync();

                if (!products.Any())
                {
                    Console.WriteLine("No products found");
                    return;
                }

                foreach (var product in products)
                {
                    Console.WriteLine(product.ProductID + " | " + product.Name + " | ");
                }

                Console.WriteLine("ProductID: ");
                if (!int.TryParse(Console.ReadLine(), out var OproductID))
                {
                    Console.WriteLine("Product not found");
                    continue;
                }

                Console.Write("Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out var Oquantity) || Oquantity <= 0)
                {
                    Console.WriteLine("Invalid input of quantity");
                    continue;
                }

                var row = new OrderRow
                {
                    ProductID = OproductID,
                    Quantity = Oquantity,
                };

                orderRows.Add(row);
            }

            order.OrderRows = orderRows;
            order.TotalAmount = orderRows.Sum(o => o.UnitPrice * o.Quantity);

            db.Orders.Add(order);

            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Order " + order.OrderID + "created!");
            } catch (DbUpdateException exception)
            {
                Console.WriteLine("DB Error: " + exception.GetBaseException().Message);
            }

            break;

        // Chosen entity = orderRows
        case "orderrows":
            Console.WriteLine("Add a row to an order\nOrder ID: ");
            if (!int.TryParse(Console.ReadLine(), out var ORorderID) ||
                !db.Orders.Any(o => o.OrderID == ORorderID))
            {
                Console.WriteLine("Error: Unknown Order ID");
            }

            // Add product to row
            Console.WriteLine("Add a product to the row by its ID: ");
            await ListAsync("products");
            if (!int.TryParse(Console.ReadLine(), out var ORproductID) ||
                !db.Products.Any(p => p.ProductID == ORproductID))
            {
                Console.WriteLine("Error: Unknown Product ID");
            }

            // Set quantity 
            Console.WriteLine("Quantity of the product: ");
            if (!int.TryParse(Console.ReadLine(), out var ORquantity) || ORquantity <= 0)
            {
                Console.WriteLine("Invalid input of quantity");
                return;
            }


            break;

        // Chosen entity = products
        case "products":

            break;

    }
}

// EDIT - Edits a chosen entity using the entity's ID
// TODO: IMPLEMENT PRINT FOR METHODS: ALL
static async Task EditAsync(string entity, int id)
{
    using var db = new ShopContext();

    switch (entity)
    {
        // Chosen entity = customers
        case "customers":

            break;

        // Chosen entity = orders
        case "orders":

            break;

        // Chosen entity = orderRows
        case "orderrows":

            break;

        // Chosen entity = products
        case "products":

            break;

    }
}

// DELETE - Deletes a chosen entity using the entity's ID
// TODO: IMPLEMENT PRINT FOR METHODS: ALL
static async Task DeleteAsync(string entity, int id)
{
    using var db = new ShopContext();

    switch (entity)
    {
        // Chosen entity = customers
        case "customers":

            break;

        // Chosen entity = orders
        case "orders":

            break;

        // Chosen entity = orderRows
        case "orderrows":

            break;

        // Chosen entity = products
        case "products":
            var product = await db.Products.FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                Console.WriteLine("Product not found");
                return;
            }
            db.Products.Remove(product);

            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Deleted product");
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine(exception.Message);
            }

            break;

    }
}

// LIST OrderSummary
static async Task ListOrderSummary()
{
    using var db = new ShopContext();

    var summaries = await db.OrderSummaries
                            .OrderByDescending(o => o.OrderDate)
                            .ToListAsync();
    Console.WriteLine("ID | Date | Total Amount | Email");
    foreach (var summary in summaries)
    {
        Console.WriteLine(summary.OrderID + " | " + summary.OrderDate + " | " + summary.TotalAmount + " | " + summary.CustomerEmail);
    }
}

static async Task OrderCountAsync()
{
    using var db = new ShopContext();

    var orderCounts = await db.CustomerOrderCounts.AsNoTracking().ToListAsync();

    Console.WriteLine(" ID | Name | Email | Number of Orders");
    foreach (var orderCount in orderCounts)
    {
        Console.WriteLine(orderCount.CustomerID + " | " + orderCount.CustomerName + " | " + orderCount.CustomerEmail + " | " + orderCount.numberOfOrders);
    }

}

static async Task ProductSalesAsync()
{
    using var db = new ShopContext();

    var sales = await db.ProductSales.AsNoTracking().ToListAsync();

    Console.WriteLine("ID | Name | Total Quantity Sold");
    foreach (var sale in sales)
    {
        Console.WriteLine(sale.ProductID + " | " + sale.ProductName + " | " + sale.TotalQuantitySold);
    }
}

static async Task ClearAll()
{
    Console.WriteLine("Are you sure you want to clear all entries in the system? (Y/N)");
    if (!Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase))
    {
        return;
    }

    using var db = new ShopContext();

    try
    {
        Console.WriteLine("Deleting all entries...");

        Console.WriteLine("Deleting all Order Rows");
        await db.OrderRows.ExecuteDeleteAsync();

        Console.WriteLine("Deleting all orders");
        await db.Orders.ExecuteDeleteAsync();

        Console.WriteLine("Deleting all products");
        await db.Products.ExecuteDeleteAsync();

        Console.WriteLine("Deleting all customers");
        await db.Customers.ExecuteDeleteAsync();

        Console.WriteLine("Successfully deleted all entries\nTo re-seed entires, exit and restart.");
    }

    catch (DbUpdateException exception)
    {
        Console.WriteLine(exception.Message);
    }

}