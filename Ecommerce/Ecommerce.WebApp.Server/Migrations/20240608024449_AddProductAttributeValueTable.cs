using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.WebApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAttributeValueTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAttributeValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductAttributeMappingId = table.Column<int>(type: "integer", nullable: false),
                    AttributeValueTypeId = table.Column<int>(type: "integer", nullable: false),
                    AssociatedProductId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ColorSquaresRgb = table.Column<string>(type: "text", nullable: true),
                    ImageSquaresPictureId = table.Column<int>(type: "integer", nullable: false),
                    PriceAdjustment = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    PriceAdjustmentUsePercentage = table.Column<bool>(type: "boolean", nullable: false),
                    WeightAdjustment = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    CustomerEntersQty = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    IsPreSelected = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    AttributeValueType = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_ProductAttributeMappings_ProductAttr~",
                        column: x => x.ProductAttributeMappingId,
                        principalTable: "ProductAttributeMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_ProductAttributeMappingId",
                table: "ProductAttributeValues",
                column: "ProductAttributeMappingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributeValues");
        }
    }
}
