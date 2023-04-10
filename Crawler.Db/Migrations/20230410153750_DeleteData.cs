using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crawler.UrlDatabase.Migrations
{
    /// <inheritdoc />
    public partial class DeleteData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM FoundUrls");
            migrationBuilder.Sql("DELETE FROM DomainUrls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
