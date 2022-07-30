using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackChain.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Index = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    PreviousBlockHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nonce = table.Column<long>(type: "bigint", nullable: false),
                    Difficulty = table.Column<long>(type: "bigint", nullable: false),
                    CurrentBlockHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Index);
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
                    Value = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlockIndex = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Transactions_Blocks_BlockIndex",
                        column: x => x.BlockIndex,
                        principalTable: "Blocks",
                        principalColumn: "Index");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BlockIndex",
                table: "Transactions",
                column: "BlockIndex");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
