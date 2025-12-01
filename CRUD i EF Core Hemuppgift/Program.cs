using CRUD_i_EF_Core_Hemuppgift;
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


    // CRUD: Create, Read, Update, Delete
    while (true)
    {
        Console.WriteLine("\nUsage: <entity> <command> (Example: customers list)");
        Console.WriteLine("Entities: customers | orders | orderRows | products | exit");
        Console.WriteLine("Commands: list | add | delete <id> | edit <id> | summary (for orders only)");
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
                Console.WriteLine(customer.Name + "'s orders:");
                foreach (var order in customer.Orders)
                {
                    Console.Write(order.OrderID + " | " + order.TotalAmount);
                }
            }

            break;

        // Chosen entity = orders
        case "orders":
            var orders = await db.Orders
                                    .AsNoTracking()
                                    .Include(order => order.Customer)
                                    .OrderBy(orders => orders.OrderID)
                                    .ToListAsync();

            Console.WriteLine("ID | Customer | Order Date | Status | Total Amount | Rows in Order");

            foreach (var order in orders)
            {
                Console.WriteLine(order.OrderID + " | " + order.Customer?.Name + " | " + order.OrderDate + " | " + order.Status + " | " + order.OrderRows?.Count);
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
                Console.WriteLine(orderrow.OrderRowID + " | " + orderrow.Order?.OrderID + " | " + orderrow.Product?.Name + " | " + orderrow.Quantity + " | " + orderrow.UnitPrice);
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
                if (!int.TryParse(Console.ReadLine(), out var productID))
                {
                    Console.WriteLine("Product not found");
                    continue;
                }

                Console.Write("Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid input of quantity");
                    continue;
                }

                var row = new OrderRow
                {
                    ProductID = productID,
                    Quantity = quantity,
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