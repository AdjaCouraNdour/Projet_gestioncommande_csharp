using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestion_commande.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commande_Clients_ClientId",
                table: "Commande");

            migrationBuilder.DropForeignKey(
                name: "FK_ProduitsCommandes_Commande_CommandeId",
                table: "ProduitsCommandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Clients_ClientId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Comptables");

            migrationBuilder.DropTable(
                name: "Rss");

            migrationBuilder.DropTable(
                name: "Secretaires");

            migrationBuilder.DropIndex(
                name: "IX_Users_ClientId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commande",
                table: "Commande");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Actif",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "Commande",
                newName: "Commandes");

            migrationBuilder.RenameIndex(
                name: "IX_Commande_ClientId",
                table: "Commandes",
                newName: "IX_Commandes_ClientId");

            migrationBuilder.AddColumn<int>(
                name: "EtatProduit",
                table: "Produits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Libelle",
                table: "Produits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Prix",
                table: "Produits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "QteStock",
                table: "Produits",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CommandeId",
                table: "Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProduitId",
                table: "Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "QteCommande",
                table: "Details",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Commandes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Commandes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EtatCommande",
                table: "Commandes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Montant",
                table: "Commandes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MontantRestant",
                table: "Commandes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MontantVerse",
                table: "Commandes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commandes",
                table: "Commandes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Livreurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomComplet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livreurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paiements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommandeId = table.Column<int>(type: "int", nullable: false),
                    Montant = table.Column<double>(type: "float", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paiements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paiements_Commandes_CommandeId",
                        column: x => x.CommandeId,
                        principalTable: "Commandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Details_CommandeId",
                table: "Details",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_ProduitId",
                table: "Details",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_CommandeId",
                table: "Paiements",
                column: "CommandeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Clients_ClientId",
                table: "Commandes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Commandes_CommandeId",
                table: "Details",
                column: "CommandeId",
                principalTable: "Commandes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Produits_ProduitId",
                table: "Details",
                column: "ProduitId",
                principalTable: "Produits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProduitsCommandes_Commandes_CommandeId",
                table: "ProduitsCommandes",
                column: "CommandeId",
                principalTable: "Commandes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Clients_ClientId",
                table: "Commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Details_Commandes_CommandeId",
                table: "Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Details_Produits_ProduitId",
                table: "Details");

            migrationBuilder.DropForeignKey(
                name: "FK_ProduitsCommandes_Commandes_CommandeId",
                table: "ProduitsCommandes");

            migrationBuilder.DropTable(
                name: "Livreurs");

            migrationBuilder.DropTable(
                name: "Paiements");

            migrationBuilder.DropIndex(
                name: "IX_Details_CommandeId",
                table: "Details");

            migrationBuilder.DropIndex(
                name: "IX_Details_ProduitId",
                table: "Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commandes",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "EtatProduit",
                table: "Produits");

            migrationBuilder.DropColumn(
                name: "Libelle",
                table: "Produits");

            migrationBuilder.DropColumn(
                name: "Prix",
                table: "Produits");

            migrationBuilder.DropColumn(
                name: "QteStock",
                table: "Produits");

            migrationBuilder.DropColumn(
                name: "CommandeId",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "ProduitId",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "QteCommande",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "EtatCommande",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "Montant",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "MontantRestant",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "MontantVerse",
                table: "Commandes");

            migrationBuilder.RenameTable(
                name: "Commandes",
                newName: "Commande");

            migrationBuilder.RenameIndex(
                name: "IX_Commandes_ClientId",
                table: "Commande",
                newName: "IX_Commande_ClientId");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Actif",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Commande",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commande",
                table: "Commande",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Comptables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comptables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Secretaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secretaires", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientId",
                table: "Users",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commande_Clients_ClientId",
                table: "Commande",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProduitsCommandes_Commande_CommandeId",
                table: "ProduitsCommandes",
                column: "CommandeId",
                principalTable: "Commande",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Clients_ClientId",
                table: "Users",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
