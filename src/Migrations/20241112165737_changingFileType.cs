using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class changingFileType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadedFile_ApplicationItems_ApplicationItemId",
                table: "UploadedFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadedFile",
                table: "UploadedFile");

            migrationBuilder.RenameTable(
                name: "UploadedFile",
                newName: "UploadedFiles");

            migrationBuilder.RenameIndex(
                name: "IX_UploadedFile_ApplicationItemId",
                table: "UploadedFiles",
                newName: "IX_UploadedFiles_ApplicationItemId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "UploadedFiles",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadedFiles",
                table: "UploadedFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadedFiles_ApplicationItems_ApplicationItemId",
                table: "UploadedFiles",
                column: "ApplicationItemId",
                principalTable: "ApplicationItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadedFiles_ApplicationItems_ApplicationItemId",
                table: "UploadedFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadedFiles",
                table: "UploadedFiles");

            migrationBuilder.RenameTable(
                name: "UploadedFiles",
                newName: "UploadedFile");

            migrationBuilder.RenameIndex(
                name: "IX_UploadedFiles_ApplicationItemId",
                table: "UploadedFile",
                newName: "IX_UploadedFile_ApplicationItemId");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "UploadedFile",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadedFile",
                table: "UploadedFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadedFile_ApplicationItems_ApplicationItemId",
                table: "UploadedFile",
                column: "ApplicationItemId",
                principalTable: "ApplicationItems",
                principalColumn: "Id");
        }
    }
}
