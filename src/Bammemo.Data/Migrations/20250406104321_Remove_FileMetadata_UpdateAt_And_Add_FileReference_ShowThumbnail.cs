using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bammemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Remove_FileMetadata_UpdateAt_And_Add_FileReference_ShowThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "FileMetadata");

            migrationBuilder.AddColumn<bool>(
                name: "ShowThumbnail",
                table: "FileReferences",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowThumbnail",
                table: "FileReferences");

            migrationBuilder.AddColumn<long>(
                name: "UpdateAt",
                table: "FileMetadata",
                type: "INTEGER",
                nullable: true);
        }
    }
}
