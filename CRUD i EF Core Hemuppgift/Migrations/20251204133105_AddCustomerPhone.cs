using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_i_EF_Core_Hemuppgift.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Customers");
        }
    }
}
