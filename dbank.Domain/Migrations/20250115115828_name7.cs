using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBank.Domain.Migrations
{
    /// <inheritdoc />
    public partial class name7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InterestRate",
                table: "Credits",
                newName: "MonthlyPayment");

            migrationBuilder.AlterColumn<int>(
                name: "CreditPeriod",
                table: "Credits",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InitialPaymentRate",
                table: "Credits",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InterestCreditRate",
                table: "Credits",
                type: "numeric",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepositPeriod",
                table: "CashDeposits",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialPaymentRate",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "InterestCreditRate",
                table: "Credits");

            migrationBuilder.RenameColumn(
                name: "MonthlyPayment",
                table: "Credits",
                newName: "InterestRate");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditPeriod",
                table: "Credits",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DepositPeriod",
                table: "CashDeposits",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
