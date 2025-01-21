using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBank.Domain.Migrations
{
    /// <inheritdoc />
    public partial class name5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                table: "CashDeposits",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AccruedInterest",
                table: "CashDeposits",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                table: "CashDeposits",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccruedInterest",
                table: "CashDeposits");

            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "CashDeposits");

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                table: "CashDeposits",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
