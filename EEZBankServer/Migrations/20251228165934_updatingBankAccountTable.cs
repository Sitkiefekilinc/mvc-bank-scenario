using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EEZBankServer.Migrations
{
    /// <inheritdoc />
    public partial class updatingBankAccountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Islemler_Hesaplar_AliciBankaHesabiId",
                table: "Islemler");

            migrationBuilder.DropForeignKey(
                name: "FK_Islemler_Hesaplar_GonderenBankaHesabiId",
                table: "Islemler");

            migrationBuilder.RenameColumn(
                name: "GonderenBankaHesabiId",
                table: "Islemler",
                newName: "GonderenBankaHesabiHesapId");

            migrationBuilder.RenameColumn(
                name: "AliciBankaHesabiId",
                table: "Islemler",
                newName: "AliciBankaHesabiHesapId");

            migrationBuilder.RenameIndex(
                name: "IX_Islemler_GonderenBankaHesabiId",
                table: "Islemler",
                newName: "IX_Islemler_GonderenBankaHesabiHesapId");

            migrationBuilder.RenameIndex(
                name: "IX_Islemler_AliciBankaHesabiId",
                table: "Islemler",
                newName: "IX_Islemler_AliciBankaHesabiHesapId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Hesaplar",
                newName: "HesapId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Islemler_Hesaplar_AliciBankaHesabiHesapId",
                table: "Islemler");

            migrationBuilder.DropForeignKey(
                name: "FK_Islemler_Hesaplar_GonderenBankaHesabiHesapId",
                table: "Islemler");

            migrationBuilder.RenameColumn(
                name: "GonderenBankaHesabiHesapId",
                table: "Islemler",
                newName: "GonderenBankaHesabiId");

            migrationBuilder.RenameColumn(
                name: "AliciBankaHesabiHesapId",
                table: "Islemler",
                newName: "AliciBankaHesabiId");

            migrationBuilder.RenameIndex(
                name: "IX_Islemler_GonderenBankaHesabiHesapId",
                table: "Islemler",
                newName: "IX_Islemler_GonderenBankaHesabiId");

            migrationBuilder.RenameIndex(
                name: "IX_Islemler_AliciBankaHesabiHesapId",
                table: "Islemler",
                newName: "IX_Islemler_AliciBankaHesabiId");

            migrationBuilder.RenameColumn(
                name: "HesapId",
                table: "Hesaplar",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Islemler_Hesaplar_AliciBankaHesabiId",
                table: "Islemler",
                column: "AliciBankaHesabiId",
                principalTable: "Hesaplar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Islemler_Hesaplar_GonderenBankaHesabiId",
                table: "Islemler",
                column: "GonderenBankaHesabiId",
                principalTable: "Hesaplar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
