using CRUD_i_EF_Core_Hemuppgift;
using CRUD_i_EF_Core_Hemuppgift.Migrations;
using CRUD_i_EF_Core_Hemuppgift.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

Console.WriteLine("Assignment for database management course.\nThis application uses both JSON file storage and EF Core with SQLite for data persistence.");
while (true)
{
    Console.WriteLine("Choose which one to use:\n1. JSON\n2. SQLite");
    Console.Write("> ");
    var initialChoice = Console.ReadLine()?.Trim().ToLowerInvariant();

    // USING LOCAL JSON FILE STORAGE
    if (initialChoice == "1" || initialChoice == "json")
    {
        Console.WriteLine("Using JSON file storage.");

        var cService = new CustomerService();
        var pService = new ProductService();
        var oService = new OrderService();

        Console.WriteLine("\nChoose entity to manage: 1 = customers | 2 = orders | 3 = products | 4 = exit");
        var choice = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(choice))
        {
            continue; // Do nothing on empty input
        }

        if (choice.Equals("4", StringComparison.OrdinalIgnoreCase))
        {
            return; // Back to database choice
        }

        while (true)
        {
            Console.WriteLine("\n1 = list | 2 = add | 3 = update | 4 = delete | 5 = back");

            // Customers
            if (choice == "1")
            {
                Console.WriteLine($"Additional for customers: 6 = list per city");
                Console.Write("> ");
                var input = Console.ReadLine()?.Trim();
                if (input == "5") break;

                switch (input)
                {
                    case "1":
                        await ListCustomerAsync(cService);
                        break;
                    case "2":
                        await AddCustomersAsync(cService);
                        break;
                    case "3":
                        await UpdateCustomerAsync(cService);
                        break;
                    case "4":
                        await DeleteCustomerAsync(cService);
                        break;
                    case "6":
                        await ListPerCityAsync(cService);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }

            // Orders
            else if (choice == "2")
            {
                //Console.Write($"Additional for orders: <TODO: ADDITIONAL CRUD FOR ORDERS GOES HERE>");
                Console.Write("> ");
                var input = Console.ReadLine()?.Trim();
                if (input == "5") break;

                switch (input)
                {
                    case "1":
                        await ListOrdersAsync(oService);
                        break;
                    case "2":
                        await AddOrderAsync(oService, cService, pService);
                        break;
                    case "3":
                        await UpdateOrderAsync(oService);
                        break;
                    case "4":
                        await DeleteOrderAsync(oService);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }

            // Products
            else if (choice == "3")
            {
                //Console.Write($"Additional for products: <TODO: ADDITIONAL CRUD FOR PRODUCTS GOES HERE>");
                Console.Write("> ");
                var input = Console.ReadLine()?.Trim();
                if (input == "5") break;

                switch (input)
                {
                    case "1":
                        await ListProductsAsync(pService);
                        break;
                    case "2":
                        await AddProductAsync(pService);
                        break;
                    case "3":
                        await UpdateProductAsync(pService);
                        break;
                    case "4":
                        await DeleteProductAsync(pService);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid entity choice");
                break;
            }
        }

        static async Task DeleteCustomerAsync(CustomerService customerService)
        {
            var customers = await customerService.LoadAllAsync();
            if (!customers.Any())
            {
                Console.WriteLine("No customers");
                return;
            }
            await ListCustomerAsync(customerService);

            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Invalid customer ID");
                return;
            }
            await customerService.DeleteAsync(ID);
            Console.WriteLine("Customer deleted");
        }

        static async Task UpdateCustomerAsync(CustomerService customerService)
        {
            var customers = await customerService.LoadAllAsync();
            if (!customers.Any())
            {
                Console.WriteLine("No customers");
                return;
            }
            await ListCustomerAsync(customerService);

            Console.Write("Customer to edit (by ID): ");
            var input = int.TryParse(Console.ReadLine(), out int ID);
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? string.Empty;
            Console.Write("City: ");
            var city = Console.ReadLine() ?? string.Empty;
            Console.Write("Phone: ");
            var phone = Console.ReadLine() ?? string.Empty;

            await customerService.UpdateAsync(ID, name, email, city, phone);
            Console.WriteLine("Customer updated");
        }
        static async Task AddCustomersAsync(CustomerService customerService)
        {
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";
            Console.Write("City: ");
            var city = Console.ReadLine() ?? "";
            Console.Write("Phone: ");
            var phone = Console.ReadLine() ?? "";

            var customer = new Customer { Name = name, Email = email, City = city, Phone = phone };
            await customerService.AddAsync(customer);
            Console.WriteLine("Customer added");
        }

        static async Task ListCustomerAsync(CustomerService customerService)
        {
            var customers = await customerService.LoadAllAsync();
            if (!customers.Any())
            {
                Console.WriteLine("No customers");
                return;
            }

            Console.WriteLine("ID | Name | Email | City | Phone");
            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.CustomerID} | {customer.Name} | {customer.Email} | {customer.City} | {customer.Phone}");
            }
        }
        static async Task ListPerCityAsync(CustomerService customerService)
        {
            var customers = await customerService.LoadAllAsync();
            if (!customers.Any())
            {
                Console.WriteLine("No customers");
                return;
            }

            Console.Write("City: ");
            var city = Console.ReadLine()?.Trim();
            var sorted = customers
                            .Where(c => c.City == city)
                            .OrderByDescending(c => c.CustomerID);

            if (!sorted.Any())
            {
                Console.WriteLine("No customers in chosen city");
                return;
            }

            Console.WriteLine("ID | Name | Email | City | Phone");
            foreach (var customer in sorted)
            {
                Console.WriteLine($"{customer.CustomerID} | {customer.Name} | {customer.Email} | {customer.City} | {customer.Phone}");
            }
        }

        static async Task DeleteOrderAsync(OrderService orderService)
        {
            var orders = await orderService.LoadAllAsync();
            if (!orders.Any())
            {
                Console.WriteLine("No orders");
                return;
            }
            await ListOrdersAsync(orderService);

            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Invalid order ID");
                return;
            }
            await orderService.DeleteAsync(ID);
            Console.WriteLine("Order deleted");
        }

        static async Task UpdateOrderAsync(OrderService orderService)
        {
            var orders = await orderService.LoadAllAsync();
            if (!orders.Any())
            {
                Console.WriteLine("No orders");
                return;
            }
            await ListOrdersAsync(orderService);

            Console.Write("Order to edit (by ID): ");
            var input = int.TryParse(Console.ReadLine(), out int ID);

            Console.Write("Status, valid inputs: 'Pending', 'Completed', 'Cancelled' or leave blank to not change: ");
            string? status = Console.ReadLine();
            if (status?.ToLower() != "pending" || Console.ReadLine() != "completed" || Console.ReadLine() != "cancelled")
            {
                Console.WriteLine("Invalid status, leaving blank");
                status = string.Empty;
            }

            Console.Write("Change order date by amount in days (accepts negative amounts): ");
            if (!int.TryParse(Console.ReadLine(), out int changeDays))
            {
                Console.WriteLine("Invalid days input, order date unchanged");
                changeDays = 0;
            }

            var orderRows = new List<OrderRow> ();
            Console.Write("Change rows in this order? Y/N");
            if (Console.ReadLine().ToLower() == "y")
            {

            }

            await orderService.UpdateAsync(ID, status, changeDays, orderRows);
            Console.WriteLine("Product updated");
        }

        static async Task AddOrderAsync(OrderService orderService, CustomerService customerService, ProductService productService)
        {
            Console.Write("Set customer to order by ID: ");
            await ListCustomerAsync(customerService);
            var customers = await customerService.LoadAllAsync();
            if (!int.TryParse(Console.ReadLine(), out int customerID)) 
            {
                Console.WriteLine("Invalid customer ID");
                return;
            }
            var customer = customers.FirstOrDefault(c => c.CustomerID == customerID);

            Console.Write("Status, valid inputs: 'Pending', 'Completed', 'Cancelled': ");
            string statusValidation = Console.ReadLine();
            if (statusValidation?.ToLower() != "pending" || Console.ReadLine() != "completed" || Console.ReadLine() != "cancelled")
            {
                Console.WriteLine("Invalid status");
                return;
            }
            var status = statusValidation;

            var order = new Order { Status = status, OrderDate = DateTime.Now };
            customer.Orders.Add(order);

            Console.Write("Add a row to order? Y/N: ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                while (true)
                {
                    Console.WriteLine("What product by ID: ");
                    await ListProductsAsync(productService);
                    var products = await productService.LoadAllAsync();
                    if (!int.TryParse(Console.ReadLine(), out int productID))
                    {
                        Console.WriteLine("Invalid product ID");
                        return;
                    }
                    var product = products.FirstOrDefault(p => p.ProductID == productID);

                    Console.Write("Quantity of product: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        Console.WriteLine("Invalid quantity");
                        return;
                    }

                    var orderRow = new OrderRow { ProductID = productID, Quantity = quantity, UnitPrice = product.Price };
                    order.OrderRows.Add(orderRow);

                    Console.Write("Add another row to order? Y/N: ");
                    if (Console.ReadLine()?.ToLower() == "n")
                    {
                        break;
                    }
                }
            }

            order.TotalAmount = order.OrderRows.Sum(or => or.Quantity * or.UnitPrice);

            await orderService.AddAsync(order);
            await customerService.AddOrderToCustomer(customerID, order);
            Console.WriteLine("Order added");
        }

        static async Task ListOrdersAsync(OrderService orderService)
        {
            var orders = await orderService.LoadAllAsync();
            if (!orders.Any())
            {
                Console.WriteLine("No orders");
                return;
            }

            Console.WriteLine("ID | Status | Date | Total Amount | Rows in order");
            foreach (var order in orders)
            {
                Console.WriteLine($"{order.OrderID} | {order.Status} | {order.OrderDate} | {order.TotalAmount} | {order.OrderRows.Count()} ");
                Console.WriteLine($" - Order Rows: ProductID | Quantity | Unit Price");
                foreach (var row in order.OrderRows)
                {
                    Console.WriteLine($" - {row.ProductID} | {row.Quantity} | {row.UnitPrice}");
                }
            }
        }

        static async Task DeleteProductAsync(ProductService productService)
        {
            var products = await productService.LoadAllAsync();
            if (!products.Any())
            {
                Console.WriteLine("No products");
                return;
            }
            await ListProductsAsync(productService);

            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Invalid product ID");
                return;
            }
            await productService.DeleteAsync(ID);
            Console.WriteLine("Product deleted");
        }

        static async Task UpdateProductAsync(ProductService productService)
        {
            var products = await productService.LoadAllAsync();
            if (!products.Any())
            {
                Console.WriteLine("No products");
                return;
            }
            await ListProductsAsync(productService);

            Console.Write("Product to edit (by ID): ");
            var input = int.TryParse(Console.ReadLine(), out int ID);
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Price: ");
            var price = Console.ReadLine() ?? string.Empty;

            await productService.UpdateAsync(ID, name, price);
            Console.WriteLine("Product updated");
        }
        static async Task AddProductAsync(ProductService productService)
        {
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine() ?? "", out var price))
            {
                Console.WriteLine("Invalid price input");
                return;
            }

            var product = new Product { Name = name, Price = price};
            await productService.AddAsync(product);
            Console.WriteLine("Customer added");
        }

        static async Task ListProductsAsync(ProductService productService)
        {
            var products = await productService.LoadAllAsync();
            if (!products.Any())
            {
                Console.WriteLine("No products");
                return;
            }

            Console.WriteLine("ID | Name | Price");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductID} | {product.Name} | {product.Price} ");
            }
        }

    }

    // USING SQLITE DATABASE
    else if (initialChoice == "2" || initialChoice == "sqlite")
    {
        Console.WriteLine("Using SQLite database with EF Core.");

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
                List<string> salts = new();
                for (int i = 0; i < 3; i++)
                salts.Add(EncryptionHelper.GenerateSalt());

                db.Customers.AddRange(
                    new Customer
                    {
                        Name = "Erik",
                        Email = "erik.stattebratt@gmail.com",
                        City = "Malmö",
                        Phone = EncryptionHelper.HashWithSalt("070515-ERIK", salts[0])
                    },
                    new Customer
                    {
                        Name = "Adam",
                        Email = "adam.fillibang@gmail.com",
                        City = "Stockholm",
                        Phone = EncryptionHelper.HashWithSalt("070515-ADAM", salts[1])
                    },
                    new Customer
                    {
                        Name = "Lisa",
                        Email = "lisa.foley@gmail.com",
                        City = "Gothenburg",
                        Phone = EncryptionHelper.HashWithSalt("070515-LISA", salts[2])
                    }
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
                    new Product { Name = "Pencil", Price = 9M },
                    new Product { Name = "Notebook", Price = 19M },
                    new Product { Name = "Eraser", Price = 4M },
                    new Product { Name = "Ruler", Price = 14M },
                    new Product { Name = "Drill", Price = 99M },
                    new Product { Name = "Saw", Price = 79M },
                    new Product { Name = "Level", Price = 24M },
                    new Product { Name = "Tape Measure", Price = 34M },
                    new Product { Name = "Chisel", Price = 44M }
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
                var orderRows3 = new List<OrderRow>
        {
            new OrderRow { ProductID = 5, Quantity = 10, UnitPrice = 19M},
            new OrderRow { ProductID = 6, Quantity = 6, UnitPrice = 4M}
        };

                db.Orders.AddRange(
                    new Order { OrderDate = DateTime.Now.AddDays(-14), Status = "Pending", CustomerID = 1, OrderRows = orderRows1, TotalAmount = orderRows1.Sum(o => o.UnitPrice * o.Quantity) },
                    new Order { OrderDate = DateTime.Now.AddDays(-30), Status = "Pending", CustomerID = 2, OrderRows = orderRows2, TotalAmount = orderRows2.Sum(o => o.UnitPrice * o.Quantity) },
                    new Order { OrderDate = DateTime.Now, Status = "Pending", CustomerID = 3, OrderRows = orderRows3, TotalAmount = orderRows3.Sum(o => o.UnitPrice * o.Quantity) }
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
                if (choice.Equals("clearall", StringComparison.OrdinalIgnoreCase))
                {
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
    }
    else
    {
        Console.WriteLine("Invalid command");
        continue;
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
                var queryCustomers = db.Customers
                                        .AsNoTracking()
                                        .Include(customers => customers.Orders)
                                        .OrderBy(customers => customers.CustomerID);

                // Show all results if less than 5 customers
                if (await queryCustomers.CountAsync() < 5)
                {
                    var customers = await queryCustomers.ToListAsync();

                    Console.WriteLine("ID | Name | Email | City | Phone (Hashed) | Total Orders");

                    foreach (var customer in customers)
                    {
                        Console.WriteLine(customer.CustomerID + " | " + customer.Name + " | " + customer.Email + " | " + customer.City + " | " + customer.Phone + " | " + customer.Orders?.Count);
                        Console.WriteLine(" - " + customer.Name + "'s orders: Order ID | Cost of Order");
                        foreach (var order in customer.Orders)
                        {
                            Console.WriteLine(" - " + order.OrderID + " | " + order.TotalAmount);
                        }
                    }
                }

                // Show paged results if more than 5 customers
                else if (await queryCustomers.CountAsync() >= 5)
                {
                    int page = 1;
                    var totalCount = await queryCustomers.CountAsync();
                    var totalPages = (int)Math.Ceiling(totalCount / 5.0);

                    while (true)
                    {
                        var customers = await queryCustomers
                        .Skip((page - 1) * 5)
                        .Take(5)
                        .ToListAsync();

                        // Show paged customers
                        Console.WriteLine("ID | Name | Email | City | Phone (Hashed) | Total Orders");

                        foreach (var customer in customers)
                        {
                            Console.WriteLine(customer.CustomerID + " | " + customer.Name + " | " + customer.Email + " | " + customer.City + " | " + customer.Phone + " | " + customer.Orders?.Count);
                            Console.WriteLine(" - " + customer.Name + "'s orders: Order ID | Cost of Order");
                            foreach (var order in customer.Orders)
                            {
                                Console.WriteLine(" - " + order.OrderID + " | " + order.TotalAmount);
                            }
                        }

                        Console.WriteLine("Page " + page + "/" + totalPages + ". Commands: previousPage, nextPage, back");

                        var command = Console.ReadLine()?.Trim().ToLowerInvariant();
                        if (command == "nextpage" && page < totalPages)
                        {
                            page++;
                        }
                        else if (command == "previouspage" && page > 1)
                        {
                            page--;
                        }
                        else if (command == "back")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                            continue;
                        }
                    }
                }
                break;

            // Chosen entity = orders
            case "orders":

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var queryOrders = db.Orders
                                        .AsNoTracking()
                                        .Include(order => order.Customer)
                                        .Include(order => order.OrderRows)
                                        .OrderBy(order => order.OrderID);
            sw.Stop();
            Console.WriteLine($"Query time: {sw.ElapsedMilliseconds} ms");

            // Show all results if less than 5 orders
            if (await queryOrders.CountAsync() < 5)
                {
                    var orders = await queryOrders.ToListAsync();

                    Console.WriteLine("ID | Customer | Order Date | Status | Total Amount | Rows in Order");

                    foreach (var order in orders)
                    {
                        Console.WriteLine(order.OrderID + " | " + order.Customer?.Name + " | " + order.OrderDate + " | " + order.Status + " | " + order.TotalAmount + " | " + order.OrderRows?.Count);
                    }
                }

                // Show paged results if more than 5 orders
                else if (await queryOrders.CountAsync() >= 5)
                {
                    int page = 1;
                    var totalCount = await queryOrders.CountAsync();
                    var totalPages = (int)Math.Ceiling(totalCount / 5.0);

                    while (true)
                    {
                        var orders = await queryOrders
                            .Skip((page - 1) * 5)
                            .Take(5)
                            .ToListAsync();

                        // Show paged orders
                        Console.WriteLine("ID | Customer | Order Date | Status | Total Amount | Rows in Order");

                        foreach (var order in orders)
                        {
                            Console.WriteLine(order.OrderID + " | " + order.Customer?.Name + " | " + order.OrderDate + " | " + order.Status + " | " + order.TotalAmount + " | " + order.OrderRows?.Count);
                        }

                        Console.WriteLine("Page " + page + "/" + totalPages + ". Commands: previousPage, nextPage, back");

                        var command = Console.ReadLine()?.Trim().ToLowerInvariant();
                        if (command == "nextpage" && page < totalPages)
                        {
                            page++;
                        }
                        else if (command == "previouspage" && page > 1)
                        {
                            page--;
                        }
                        else if (command == "back")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                            continue;
                        }
                    }
                }
                break;

            // Chosen entity = orderRows
            case "orderrows":
                var queryOrderrows = db.OrderRows
                                        .AsNoTracking()
                                        .Include(orderr => orderr.Product)
                                        .OrderBy(orderr => orderr.OrderID);

                // show all results if less than 5 order rows
                Console.WriteLine("ID | Exists in OrderID: | Product | Quantity | Unit Price");
                if (await queryOrderrows.CountAsync() < 5)
                {
                    var orderrows = await queryOrderrows.ToListAsync();
                    foreach (var orderrow in orderrows)
                    {
                        Console.WriteLine(orderrow.OrderRowID + " | " + orderrow.OrderID + " | " + orderrow.Product?.Name + " | " + orderrow.Quantity + " | " + orderrow.UnitPrice);
                    }
                }

                // Show paged results if more than 5 orders
                else if (await queryOrderrows.CountAsync() >= 5)
                {
                    int page = 1;
                    var totalCount = await queryOrderrows.CountAsync();
                    var totalPages = (int)Math.Ceiling(totalCount / 5.0);

                    while (true)
                    {
                        var orderrows = await queryOrderrows
                            .Skip((page - 1) * 5)
                            .Take(5)
                            .ToListAsync();

                        // Show paged orders
                        Console.WriteLine("ID | Exists in OrderID: | Product | Quantity | Unit Price");

                        foreach (var orderrow in orderrows)
                        {
                            Console.WriteLine(orderrow.OrderRowID + " | " + orderrow.OrderID + " | " + orderrow.Product?.Name + " | " + orderrow.Quantity + " | " + orderrow.UnitPrice);
                        }

                        Console.WriteLine("Page " + page + "/" + totalPages + ". Commands: previousPage, nextPage, back");

                        var command = Console.ReadLine()?.Trim().ToLowerInvariant();
                        if (command == "nextpage" && page < totalPages)
                        {
                            page++;
                        }
                        else if (command == "previouspage" && page > 1)
                        {
                            page--;
                        }
                        else if (command == "back")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                            continue;
                        }
                    }
                }
                break;

            // Chosen entity = products
            case "products":
                var queryProducts = db.Products
                                        .AsNoTracking()
                                        .OrderBy(p => p.ProductID);

                // Show all results if less than 5 products
                if (await queryProducts.CountAsync() < 5)
                {
                    var products = await queryProducts.ToListAsync();

                    Console.WriteLine("ID | Name | Price");

                    foreach (var product in products)
                    {
                        Console.WriteLine(product.ProductID + " | " + product.Name + " | " + product.Price);
                    }
                }

                // Show paged results if more than 5 products
                else if (await queryProducts.CountAsync() >= 5)
                {
                    int page = 1;
                    var totalCount = await queryProducts.CountAsync();
                    var totalPages = (int)Math.Ceiling(totalCount / 5.0);

                    while (true)
                    {
                        var products = await queryProducts
                            .Skip((page - 1) * 5)
                            .Take(5)
                            .ToListAsync();

                        // Show paged products
                        Console.WriteLine("ID | Name | Price");

                        foreach (var product in products)
                        {
                            Console.WriteLine(product.ProductID + " | " + product.Name + " | " + product.Price);
                        }

                        Console.WriteLine("Page " + page + "/" + totalPages + ". Commands: previousPage, nextPage, back");

                        var command = Console.ReadLine()?.Trim().ToLowerInvariant();
                        if (command == "nextpage" && page < totalPages)
                        {
                            page++;
                        }
                        else if (command == "previouspage" && page > 1)
                        {
                            page--;
                        }
                        else if (command == "back")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                            continue;
                        }
                    }
                }
                break;

        }
    }


    // ADD - Add 1 entry for the chosen entity
    // TODO: IMPLEMENT ADD FOR METHODS: (OrderRows, Products)
    static async Task AddAsync(string entity)
    {
        using var db = new ShopContext();
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
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

                    // Phone
                    Console.WriteLine("Enter phone number");
                    var customerPhone = Console.ReadLine()?.Trim() ?? string.Empty;

                    // Add all inputs to DB
                    db.Customers.Add(new Customer { Name = customerName, Email = customerEmail, City = customerCity, Phone = EncryptionHelper.HashWithSalt(customerPhone, EncryptionHelper.GenerateSalt())});
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

                    while (true)
                    {
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
                    Console.WriteLine("Row added to order successfully");
                    }

                    order.OrderRows = orderRows;
                    order.TotalAmount = orderRows.Sum(o => o.UnitPrice * o.Quantity);

                    db.Orders.Add(order);

                    try
                    {
                        await db.SaveChangesAsync();
                        Console.WriteLine("Order " + order.OrderID + " created");
                    }
                    catch (DbUpdateException exception)
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
                        return;
                    }

                    // Add product to row
                    Console.WriteLine("Add a product to the row by its ID: ");
                    await ListAsync("products");
                    if (!int.TryParse(Console.ReadLine(), out var ORproductID) ||
                        !db.Products.Any(p => p.ProductID == ORproductID))
                    {
                        Console.WriteLine("Error: Unknown Product ID");
                        return;
                    }

                    // Set quantity 
                    Console.WriteLine("Quantity of the product: ");
                    if (!int.TryParse(Console.ReadLine(), out var ORquantity) || ORquantity <= 0)
                    {
                        Console.WriteLine("Invalid input of quantity");
                        return;
                    }

                    var ORorder = await db.Orders.FirstOrDefaultAsync(o => o.OrderID == ORorderID);
                    var ORproduct = await db.Products.FirstOrDefaultAsync(p => p.ProductID == ORproductID);
                    var orderRow = new OrderRow
                    {
                        OrderID = ORorderID,
                        ProductID = ORproductID,
                        Quantity = ORquantity,
                        UnitPrice = ORproduct.Price
                    };
                    ORorder.OrderRows.Add(orderRow);
                    Console.WriteLine("Order row added");
                    break;

                // Chosen entity = products
                case "products":
                    Console.WriteLine("Create a new product:");
                    Console.WriteLine("Name: ");
                    var pName = Console.ReadLine()?.Trim() ?? string.Empty;
                    if (string.IsNullOrEmpty(pName) || pName.Length > 255)
                    {
                        Console.WriteLine("Name is required, and cannot exceed 255 characters");
                        return;
                    }

                    Console.WriteLine("Set a price for the product:");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal pPrice))
                    {
                        Console.WriteLine("Invalid price input");
                        return;
                    }

                    db.Products.Add(new Product { Name = pName, Price = pPrice });
                    Console.WriteLine("Product added");
                    break;
            }

            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        catch (Exception exception)
        {
        await transaction.RollbackAsync();
        Console.WriteLine(exception);
        throw;
        }
    }

    // EDIT - Edits a chosen entity using the entity's ID
    // TODO: IMPLEMENT PRINT FOR METHODS: ALL
    static async Task EditAsync(string entity, int id)
    {
        using var db = new ShopContext();
        await using var transaction = await db.Database.BeginTransactionAsync();

        switch (entity)
        {
            // Chosen entity = customers
            case "customers":
                var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerID == id);
                if (customer == null)
                {
                    Console.WriteLine("Customer not found");
                    return;
                }

                // Update customer's name
                Console.WriteLine("Current name: " + customer.Name);
                var customerName = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(customerName))
                {
                    customer.Name = customerName;
                }

                // Update customer email
                Console.WriteLine("Current email: " + customer.Email);
                var customerEmail = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(customerEmail))
                {
                    customer.Email = customerEmail;
                }

                // Update customer city
                Console.WriteLine("Current city: " + customer.City);
                var customerCity = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(customerCity))
                {
                    customer.City = customerCity;
                }

                // Update customer phone
                Console.WriteLine("Current phone: " + customer.Phone);
                var customerPhone = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(customerPhone))
                {
                    customer.Phone = customerPhone;
                }

                try
                {
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine("Edited customer info succesfully");
                }
                catch (DbUpdateException exception)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Failed to edit customer");
                    Console.WriteLine(exception.Message);
                }
            break;

            // Chosen entity = orders
            case "orders":
                var order = await db.Orders.FirstOrDefaultAsync(o => o.OrderID == id);
                if (order == null)
                {
                    Console.WriteLine("Order not found");
                    return;
                }

                // Update order status
                Console.WriteLine("Current status: " + order.Status);
                var orderStatus = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(orderStatus) && orderStatus == "Pending" || orderStatus == "Completed" || orderStatus == "Cancelled")
                {
                    order.Status = orderStatus;
                }
                else
                {
                    Console.WriteLine("Status does not match correct statuses, or was left blank. Not changing");
                }

                try
                {
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine("Edited order info succesfully");
                }
                catch (DbUpdateException exception)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Failed to edit order");
                    Console.WriteLine(exception.Message);
                }
            break;

            // Chosen entity = orderRows
            case "orderrows":
                var orderrow = await db.OrderRows.FirstOrDefaultAsync(or => or.OrderRowID == id);
                if (orderrow == null)
                {
                    Console.WriteLine("Order row not found");
                    return;
                }

                // Update row product
                Console.WriteLine("Current product: " + orderrow.Product.Name);
                var orderRowProduct = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(orderRowProduct) && db.Products.Any(p => p.Name == orderRowProduct))
                {
                    var chosenProduct = await db.Products.FirstOrDefaultAsync(p => p.Name == orderRowProduct);
                    orderrow.ProductID = chosenProduct.ProductID;
                    orderrow.UnitPrice = chosenProduct.Price;
                }

                // Update row quantity
                Console.WriteLine("Current quantity: " + orderrow.Quantity);
                var orderRowQuantityInput = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(orderRowQuantityInput) && int.TryParse(orderRowQuantityInput, out var orderRowQuantity))
                {
                    orderrow.Quantity = orderRowQuantity;
                }

                try
                {
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine("Edited order row info succesfully");
                }
                catch (DbUpdateException exception)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Failed to edit order row");
                    Console.WriteLine(exception.Message);
                }
                break;

            // Chosen entity = products
            case "products":
                var product = await db.Products.FirstOrDefaultAsync(p => p.ProductID == id);
                if (product == null)
                {
                    Console.WriteLine("Product not found");
                    return;
                }

                // Update product name
                Console.WriteLine("Current name: " + product.Name);
                var productName = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(productName))
                {
                    product.Name = productName;
                }

                // Update product price
                Console.WriteLine("Current price: " + product.Price);
                var productPriceInput = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(productPriceInput) && decimal.TryParse(productPriceInput, out var productPrice))
                {
                    product.Price = productPrice;
                }

                try
                {
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine("Edited product row info succesfully");
                }
                catch (DbUpdateException exception)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Failed to edit product");
                    Console.WriteLine(exception.Message);
                }
                break;

        }
    }

    // DELETE - Deletes a chosen entity using the entity's ID
    static async Task DeleteAsync(string entity, int id)
    {
        using var db = new ShopContext();
        await using var transaction = await db.Database.BeginTransactionAsync();

        switch (entity)
        {
            // Chosen entity = customers
            case "customers":
                var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerID == id);
                if (customer == null)
                {
                    Console.WriteLine("Customer not found");
                    return;
                }
                db.Customers.Remove(customer);
                try
                {
                    await transaction.CommitAsync();
                    await db.SaveChangesAsync();
                    Console.WriteLine("Deleted customer");
                }
                catch (DbUpdateException exception)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Failed to delete customer");
                    Console.WriteLine(exception.Message);
                }

            break;

            // Chosen entity = orders
            case "orders":
                var order = await db.Orders.FirstOrDefaultAsync(o => o.OrderID == id);
                if (order == null)
                {
                    Console.WriteLine("Order not found");
                    return;
                }
                db.Orders.Remove(order);
                try
                {
                    await transaction.CommitAsync();
                    await db.SaveChangesAsync();
                    Console.WriteLine("Deleted order");
                }
                catch (DbUpdateException exception)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Failed to delete order");
                    Console.WriteLine(exception.Message);
                }

            break;

            // Chosen entity = orderRows
            case "orderrows":
                var orderrow = await db.OrderRows.FirstOrDefaultAsync(or => or.OrderRowID == id);
                if (orderrow == null)
                {
                    Console.WriteLine("Product not found");
                    return;
                }
                db.OrderRows.Remove(orderrow);
                try
                {
                    await transaction.CommitAsync();
                    await db.SaveChangesAsync();
                    Console.WriteLine("Deleted Order Row");
                }
                catch (DbUpdateException exception)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Failed to delete orderrow");
                    Console.WriteLine(exception.Message);
                }

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
                    await transaction.CommitAsync();
                    await db.SaveChangesAsync();
                    Console.WriteLine("Deleted product");
                }
                catch (DbUpdateException exception)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Failed to delete product");
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
        var orderRows = await db.OrderRows.ToListAsync();
        var orders = await db.Orders.ToListAsync();
        var products = await db.Products.ToListAsync();
        var customers = await db.Customers.ToListAsync();

        try
        {
            Console.WriteLine("Deleting all entries...");

            Console.WriteLine("Deleting all Order Rows");
            foreach (var row in orderRows)
            {
                db.OrderRows.Remove(row);
            }
            await db.SaveChangesAsync();

            Console.WriteLine("Deleting all orders");
            foreach (var order in orders)
            {
                db.Orders.Remove(order);
            }
            await db.SaveChangesAsync();

            Console.WriteLine("Deleting all products");
            foreach (var product in products)
            {
                db.Products.Remove(product);
            }
            await db.SaveChangesAsync();

            Console.WriteLine("Deleting all customers");
            foreach (var customer in customers)
            {
                db.Customers.Remove(customer);
            }

            await db.SaveChangesAsync();
            Console.WriteLine("Resetting SQLite sequences");
            await db.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence;");
            Console.WriteLine("Successfully deleted all entries\nTo re-seed entires, exit and restart.");
        }

        catch (DbUpdateException exception)
        {
            Console.WriteLine("ERROR:");
            Console.WriteLine(exception.InnerException?.Message ?? exception.Message);
        }

    }


// Reference for adding a transaction to current methods
static async Task TransactionBlank()
{
    using var db = new ShopContext();

    await using var transaction = await db.Database.BeginTransactionAsync();

    try
    {
        // CODE
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    catch (Exception exception)
    {
        await transaction.RollbackAsync();
        Console.WriteLine(exception);
        throw;
    }
}

static async Task AddOrderWithTransactionAsync()
{
    using var db = new ShopContext();

    // Start a transaction
    await using var transaction = await db.Database.BeginTransactionAsync();

    try
    {
        var customers = await db.Customers
                                .AsNoTracking().OrderBy(o => o.CustomerID).ToListAsync();
        if (!customers.Any())
        {
            Console.WriteLine("No customers found.");
            return;
        }

        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerID} | {customer.Name}");
        }

        Console.WriteLine("CustomerID: ");
        if (!int.TryParse(Console.ReadLine(), out var customerID) ||
            !db.Customers.Any(c => c.CustomerID == customerID))
        {
            Console.WriteLine("Invalid input of CustomerID");
            return;
        }

        var products = await db.Products.AsNoTracking()
                                        .OrderBy(o => o.ProductID)
                                        .ToListAsync();

        if (!products.Any())
        {
            Console.WriteLine("No products founds");
            return;
        }

        var productLook = products.ToDictionary(p => p.ProductID, p => p);
        var orderRows = new List<OrderRow>();

        var order = new Order
        {
            CustomerID = customerID,
            OrderDate = DateTime.Now,
            Status = "Pending",
            TotalAmount = 0
        };

        while (true)
        {
            Console.Write("Add order? y/n");
            var answer = Console.ReadLine() ?? string.Empty;
            if (answer.ToLower() == "y")
                break;

            foreach (var product in products)
            {
                Console.WriteLine($"{product.Name} | {product.ProductID} | {product.Price}");
            }

            Console.Write("ProductID: ");
            if (!int.TryParse(Console.ReadLine(), out var productID))
            {
                Console.WriteLine("Invalid input of ProductID");
            }

            Console.Write("Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out var quantity))
            {
                Console.WriteLine("Invalid input of Quantity");
            }
            
            var row = new OrderRow
            {
                ProductID = productID,
                Quantity = quantity,
                UnitPrice = productLook.ContainsKey(productID) ? productLook[productID].Price : 0
            };
            order.OrderRows.Add(row);

        }

        order.OrderRows = orderRows;
        order.TotalAmount = orderRows.Sum(o => o.UnitPrice * o.Quantity);

        db.Orders.Add(order);

        // Within transaction
        await db.SaveChangesAsync();


        // Update the database with new transaction
        await transaction.CommitAsync();
    }
    catch (Exception exception)
    {
        await transaction.RollbackAsync();
        Console.WriteLine("Failed when saving order, rolling back transaction.");
        Console.WriteLine(exception);
        throw;
    }
}