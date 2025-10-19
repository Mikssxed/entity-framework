using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AdditionStateSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "States",
                column: "Message",
                value: "On Hold"
            );

            migrationBuilder.InsertData(
                table: "States",
                column: "Message",
                value: "Rejected"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "States",
                keyColumn: "Message",
                keyValue: "On Hold"
            );

            migrationBuilder.DeleteData(
                table: "States",
                keyColumn: "Message",
                keyValue: "Rejected"
            );
        }
    }
}
