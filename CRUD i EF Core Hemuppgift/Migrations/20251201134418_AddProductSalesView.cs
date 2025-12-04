using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_i_EF_Core_Hemuppgift.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSalesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS ProductSalesView AS 
            SELECT 
                p.ProductID,
                p.Name AS ProductName,
                IFNULL(SUM(orw.Quantity), 0) AS TotalQuantitySold
            FROM Products p
            LEFT JOIN OrderRows orw ON orw.ProductID = p.ProductID
            GROUP BY p.ProductID, p.Name;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS ProductSalesView
            ");
        }
    }
}
