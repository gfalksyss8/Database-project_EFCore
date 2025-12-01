using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
/*
 * En view är en sparad SELECT-fråga i db.
 *  - Förenklar komplexa JOINs
 *  - Ger oss färdiga summeringar
 *  - Slipper skriva samma SQL om och om igen
 *  - Säkert visning av information och prestanda blir bättre 
 */
namespace CRUD_i_EF_Core_Hemuppgift.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSummaryView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS OrderSummaryView AS 
            SELECT 
                o.OrderId,
                o.OrderDate,
                c.Name AS CustomerName,
                c.Email AS CustomerEmail,
                IFNULL(SUM(orw.Quantity * orw.UnitPrice), 0) AS TotalAmount
            FROM Orders o
            JOIN Customers c ON c.CustomerID = o.CustomerID
            LEFT JOIN OrderRows orw ON orw.OrderID = o.OrderID
            GROUP BY o.OrderID, o.OrderDate, c.Name, c.Email;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS OrderSummaryView
            ");
        }
    }
}
