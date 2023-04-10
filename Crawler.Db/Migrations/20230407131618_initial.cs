﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crawler.UrlDatabase.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InitialUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoundUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseTimeMs = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<int>(type: "int", nullable: false),
                    DomainUrlId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoundUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoundUrls_DomainUrls_DomainUrlId",
                        column: x => x.DomainUrlId,
                        principalTable: "InitialUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoundUrls_DomainUrlId",
                table: "FoundUrls",
                column: "InitialUrlId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoundUrls");

            migrationBuilder.DropTable(
                name: "InitialUrls");
        }
    }
}
