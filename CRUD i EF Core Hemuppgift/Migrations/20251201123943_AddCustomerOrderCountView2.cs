using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_i_EF_Core_Hemuppgift.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerOrderCountView2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS CustomerOrderCountView AS 
            SELECT 
                c.CustomerID,
                c.Name AS CustomerName,
                c.Email AS CustomerEmail,
                IFNULL(COUNT(o.OrderID), 0) AS numberOfOrders
            FROM Customers c
            LEFT JOIN Orders o ON o.CustomerID = c.CustomerID
            GROUP BY c.CustomerID, c.Name, c.Email;
            END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS CustomerOrderCountView
            ");
        }
    }
}
