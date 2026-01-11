using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCodeGeneratedRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Code_CodeGenerated",
                table: "Code",
                column: "CodeGenerated");

            migrationBuilder.CreateIndex(
                name: "IX_Code_Status",
                table: "Code",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Code_CodeGenerated",
                table: "Code");

            migrationBuilder.DropIndex(
                name: "IX_Code_Status",
                table: "Code");
        }
    }
}
