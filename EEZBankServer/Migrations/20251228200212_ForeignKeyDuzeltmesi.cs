using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EEZBankServer.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyDuzeltmesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Islemler_Hesaplar_AliciBankaHesabiHesapId",
                table: "Islemler");

            migrationBuilder.DropForeignKey(
                name: "FK_Islemler_Hesaplar_GonderenBankaHesabiHesapId",
                table: "Islemler");

            migrationBuilder.DropIndex(
                name: "IX_Islemler_AliciBankaHesabiHesapId",
                table: "Islemler");

            migrationBuilder.DropIndex(
                name: "IX_Islemler_GonderenBankaHesabiHesapId",
                table: "Islemler");

            migrationBuilder.DropColumn(
                name: "AliciBankaHesabiHesapId",
                table: "Islemler");

            migrationBuilder.DropColumn(
                name: "GonderenBankaHesabiHesapId",
                table: "Islemler");

            migrationBuilder.CreateIndex(
                name: "IX_Islemler_AliciHesapId",
                table: "Islemler",
                column: "AliciHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_Islemler_GonderenHesapId",
                table: "Islemler",
                column: "GonderenHesapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Islemler_Hesaplar_AliciHesapId",
                table: "Islemler",
                column: "AliciHesapId",
                principalTable: "Hesaplar",
                principalColumn: "HesapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Islemler_Hesaplar_GonderenHesapId",
                table: "Islemler",
                column: "GonderenHesapId",
                principalTable: "Hesaplar",
                principalColumn: "HesapId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Islemler_Hesaplar_AliciHesapId",
                table: "Islemler");

            migrationBuilder.DropForeignKey(
                name: "FK_Islemler_Hesaplar_GonderenHesapId",
                table: "Islemler");

            migrationBuilder.DropIndex(
                name: "IX_Islemler_AliciHesapId",
                table: "Islemler");

            migrationBuilder.DropIndex(
                name: "IX_Islemler_GonderenHesapId",
                table: "Islemler");

            migrationBuilder.AddColumn<Guid>(
                name: "AliciBankaHesabiHesapId",
                table: "Islemler",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GonderenBankaHesabiHesapId",
                table: "Islemler",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Islemler_AliciBankaHesabiHesapId",
                table: "Islemler",
                column: "AliciBankaHesabiHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_Islemler_GonderenBankaHesabiHesapId",
                table: "Islemler",
                column: "GonderenBankaHesabiHesapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Islemler_Hesaplar_AliciBankaHesabiHesapId",
                table: "Islemler",
                column: "AliciBankaHesabiHesapId",
                principalTable: "Hesaplar",
                principalColumn: "HesapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Islemler_Hesaplar_GonderenBankaHesabiHesapId",
                table: "Islemler",
                column: "GonderenBankaHesabiHesapId",
                principalTable: "Hesaplar",
                principalColumn: "HesapId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
