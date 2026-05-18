using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kamirion.RepairShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchIndexEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    SearchableText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplaySubtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IndexedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchIndexEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchIndexEntries_IsDeleted",
                table: "SearchIndexEntries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SearchIndexEntries_TenantId_EntityType_EntityId",
                table: "SearchIndexEntries",
                columns: new[] { "TenantId", "EntityType", "EntityId" },
                unique: true);

            // Full-Text Search catalog and index are only created when FTS is installed.
            // The standard SQL Server Docker image does not include FTS; in that environment
            // the table is created but FTS features are unavailable (graceful degradation).
            migrationBuilder.Sql(@"
                IF FULLTEXTSERVICEPROPERTY('IsFulltextInstalled') = 1
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'RepairShopCatalog')
                        CREATE FULLTEXT CATALOG RepairShopCatalog AS DEFAULT;
                END;",
                suppressTransaction: true);

            migrationBuilder.Sql(@"
                IF FULLTEXTSERVICEPROPERTY('IsFulltextInstalled') = 1
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM sys.fulltext_indexes fi
                        JOIN sys.tables t ON fi.object_id = t.object_id
                        WHERE t.name = 'SearchIndexEntries')
                    BEGIN
                        CREATE FULLTEXT INDEX ON SearchIndexEntries(SearchableText)
                            KEY INDEX PK_SearchIndexEntries
                            ON RepairShopCatalog
                            WITH CHANGE_TRACKING AUTO;
                    END;
                END;",
                suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF FULLTEXTSERVICEPROPERTY('IsFulltextInstalled') = 1
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM sys.fulltext_indexes fi
                        JOIN sys.tables t ON fi.object_id = t.object_id
                        WHERE t.name = 'SearchIndexEntries')
                        DROP FULLTEXT INDEX ON SearchIndexEntries;
                END;",
                suppressTransaction: true);

            migrationBuilder.DropTable(
                name: "SearchIndexEntries");
        }
    }
}
