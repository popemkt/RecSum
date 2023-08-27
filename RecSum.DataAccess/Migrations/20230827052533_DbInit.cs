using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecSum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    CurrencyCode = table.Column<string>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpeningValue = table.Column<double>(type: "REAL", nullable: false),
                    PaidValue = table.Column<double>(type: "REAL", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Cancelled = table.Column<bool>(type: "INTEGER", nullable: true),
                    DebtorName = table.Column<string>(type: "TEXT", nullable: false),
                    DebtorReference = table.Column<string>(type: "TEXT", nullable: false),
                    DebtorAddress1 = table.Column<string>(type: "TEXT", nullable: true),
                    DebtorAddress2 = table.Column<string>(type: "TEXT", nullable: true),
                    DebtorTown = table.Column<string>(type: "TEXT", nullable: true),
                    DebtorState = table.Column<string>(type: "TEXT", nullable: true),
                    DebtorZip = table.Column<string>(type: "TEXT", nullable: true),
                    DebtorCountryCode = table.Column<string>(type: "TEXT", nullable: false),
                    DebtorRegistrationNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Reference);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoice");
        }
    }
}
