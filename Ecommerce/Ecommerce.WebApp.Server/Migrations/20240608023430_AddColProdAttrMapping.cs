using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.WebApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddColProdAttrMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttributeControlType",
                table: "ProductAttributeMappings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttributeControlTypeId",
                table: "ProductAttributeMappings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConditionAttributeXml",
                table: "ProductAttributeMappings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "ProductAttributeMappings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductAttributeMappings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "ProductAttributeMappings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TextPrompt",
                table: "ProductAttributeMappings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidationFileAllowedExtensions",
                table: "ProductAttributeMappings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValidationFileMaximumSize",
                table: "ProductAttributeMappings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValidationMaxLength",
                table: "ProductAttributeMappings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValidationMinLength",
                table: "ProductAttributeMappings",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeControlType",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "AttributeControlTypeId",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "ConditionAttributeXml",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "TextPrompt",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "ValidationFileAllowedExtensions",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "ValidationFileMaximumSize",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "ValidationMaxLength",
                table: "ProductAttributeMappings");

            migrationBuilder.DropColumn(
                name: "ValidationMinLength",
                table: "ProductAttributeMappings");
        }
    }
}
