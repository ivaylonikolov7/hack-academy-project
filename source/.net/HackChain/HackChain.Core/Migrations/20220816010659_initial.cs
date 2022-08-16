using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackChain.Core.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Address = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false),
                    Nonce = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Address);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    PreviousBlockHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nonce = table.Column<long>(type: "bigint", nullable: false),
                    Difficulty = table.Column<long>(type: "bigint", nullable: false),
                    CurrentBlockHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerializedForMining = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nonce = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    Fee = table.Column<long>(type: "bigint", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Transactions_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_Index",
                table: "Blocks",
                column: "Index",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BlockId",
                table: "Transactions",
                column: "BlockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
