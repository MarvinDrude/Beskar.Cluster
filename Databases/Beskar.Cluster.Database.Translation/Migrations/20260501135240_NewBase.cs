using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beskar.Cluster.Database.Translation.Migrations
{
    /// <inheritdoc />
    public partial class NewBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Languages",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LangKeys",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LangGroups",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LangEntries",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LangKeys");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LangGroups");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LangEntries");
        }
    }
}
