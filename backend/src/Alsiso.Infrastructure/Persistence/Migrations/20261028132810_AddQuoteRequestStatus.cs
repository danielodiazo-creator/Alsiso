using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alsiso.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuoteRequestStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "QuoteRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "QuoteRequests");
        }
    }
}
