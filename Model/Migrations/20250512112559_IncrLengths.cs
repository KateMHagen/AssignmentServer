using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class IncrLengths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Book",
                type: "nvarchar(255)",
                maxLength: 555,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "author",
                table: "Book",
                type: "nvarchar(555)",
                maxLength: 555,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
               name: "title",
               table: "Book",
               type: "nvarchar(150)",
               maxLength: 150,
               nullable: false,
               oldClrType: typeof(string),
               oldType: "nvarchar(555)",
               oldMaxLength: 555);

            migrationBuilder.AlterColumn<string>(
                name: "author",
                table: "Book",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(555)",
                oldMaxLength: 555);
        }
    }
}
