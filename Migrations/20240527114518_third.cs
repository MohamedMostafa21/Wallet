using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Digital_Wallet.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureFileName",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureFileName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
