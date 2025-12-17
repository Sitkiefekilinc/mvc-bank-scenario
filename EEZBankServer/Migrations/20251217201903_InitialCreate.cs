using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EEZBankServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserSurname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserIban = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    UserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TcKimlikNo = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    UserBirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    HasTheAgreementBeenAccepted = table.Column<bool>(type: "bit", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "KurumsalKullaniciBilgileri",
                columns: table => new
                {
                    KurumsalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorporateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorporateType = table.Column<int>(type: "int", nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CorporateAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorizedPersonsTask = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumsalKullaniciBilgileri", x => x.KurumsalId);
                    table.ForeignKey(
                        name: "FK_KurumsalKullaniciBilgileri_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicariKullaniciBilgileri",
                columns: table => new
                {
                    TicariId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicariKullaniciBilgileri", x => x.TicariId);
                    table.ForeignKey(
                        name: "FK_TicariKullaniciBilgileri_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KurumsalKullaniciBilgileri_UserId",
                table: "KurumsalKullaniciBilgileri",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicariKullaniciBilgileri_UserId",
                table: "TicariKullaniciBilgileri",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KurumsalKullaniciBilgileri");

            migrationBuilder.DropTable(
                name: "TicariKullaniciBilgileri");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
