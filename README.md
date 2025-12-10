# Database-project_EFCore
Assignment for database management course, using either a JSON storage format or SQLite

## Running the code

To run this code, use your compiler of choice. The user interacts with the program using the console.

> [!IMPORTANT]
> Ensure you are using [.NET 8.0.414](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or newer
> NuGet Packages used: Microsoft.EntityFrameworkCore.Design (9.0.11), Microsoft.EntityFrameworkCore.Sqlite (9.0.11)

## Using the program

Starting the program using a compiler, or running it from the .EXE prompts the user with the console.
First, choose whether to interact with the JSON storage, or the SQLite Database using the number keys.
This executes and loads the chosen data.

When choosing SQLite, test information is seeded on first start-up. You can clear this from the system by using the command:
`clearall`
After that, close and re-open the program to re-seed it once more.

> [!IMPORTANT]
> If this command fails, or information is seeded incorrectly, you can manually clear the SQLite database by 
> - Closing the program and your compiler
> * Removing your local .bin and .obj folders
> + Restoring the project

The SQLite option uses a CRUD operation which allows the user to list, add, remove or edit any entires.
The program takes inputs formatted as:
`<entity> <command> <if applicable, integer>`

The JSON option uses a two-step CRUD operation using numbers. First, choose your entity to manage, then choose what operation to conduct using the numbers given by each operation.
The JSON database is stored inside:
`\CRUD i EF Core Hemuppgift\CRUD i EF Core Hemuppgift\bin\Debug\net8.0\<entity>.json`

## Additional information

The SQLite database stores and displays customer's phone numbers using Salt cryptography

The JSON system uses a 66 byte key to encrypt customer's email addresses.