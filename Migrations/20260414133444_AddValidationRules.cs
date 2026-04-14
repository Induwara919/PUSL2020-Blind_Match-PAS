using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PUSL2020_Blind_Match_PAS.Migrations
{
    /// <inheritdoc />
    public partial class AddValidationRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TechStack",
                table: "Proposals",
                newName: "TechnicalStack");

            migrationBuilder.RenameColumn(
                name: "IsRevealed",
                table: "Proposals",
                newName: "IsIdentityRevealed");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Proposals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Abstract",
                table: "Proposals",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorContact",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorName",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "SupervisorContact",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "SupervisorName",
                table: "Proposals");

            migrationBuilder.RenameColumn(
                name: "TechnicalStack",
                table: "Proposals",
                newName: "TechStack");

            migrationBuilder.RenameColumn(
                name: "IsIdentityRevealed",
                table: "Proposals",
                newName: "IsRevealed");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Abstract",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);
        }
    }
}
