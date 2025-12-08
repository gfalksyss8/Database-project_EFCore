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

            // AFTER INSERT
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Insert
            AFTER INSERT ON OrderRows
            BEGIN
                UPDATE Orders
                SET TotalAmount = (
                                SELECT IFNULL(SUM(Quantity * UnitPrice), 0) 
                                FROM OrderRows 
                                WHERE OrderID = NEW.OrderID
                                )
                    WHERE OrderID = NEW.OrderID;
                END;
            ");

            // AFTER UPDATE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Update
            AFTER UPDATE ON OrderRows
            BEGIN
                UPDATE Orders
                SET TotalAmount = ( 
                                SELECT IFNULL(SUM(Quantity * UnitPrice), 0)
                                FROM OrderRows 
                                WHERE OrderID = NEW.OrderID
                                )
                WHERE OrderID = NEW.OrderID;
            END;
            ");

            // AFTER DELETE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Delete
            AFTER DELETE ON OrderRows
            BEGIN
                Update Orders
                SET TotalAmount = ( 
                                SELECT IFNULL(SUM(Quantity * UnitPrice), 0)
                                FROM OrderRows 
                                WHERE OrderID = OLD.OrderID
                                )
                WHERE OrderID = OLD.OrderID;
            END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS OrderSummaryView
            ");

            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Insert
            ");
            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Update
            ");
            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Delete
            ");
        }
    }
}
