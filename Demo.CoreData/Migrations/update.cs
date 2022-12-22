#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.CoreData.Migrations;

/// <inheritdoc />
public partial class Update : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Article",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                ArticleName = table.Column<string>("nvarchar(500)", maxLength: 500, nullable: false),
                Status = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: false),
                CreationDate = table.Column<DateTime>("datetime2", nullable: true),
                CreationBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                RefURL = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: false),
                ImageThumb = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: true),
                Description = table.Column<string>("nvarchar(500)", maxLength: 500, nullable: true),
                CategoryId = table.Column<int>("int", nullable: false),
                IdDisplay = table.Column<int>("int", nullable: false)
            },
            constraints: table => { table.PrimaryKey("Pk_Article_Id", x => x.Id); });

        migrationBuilder.CreateTable(
            "Category",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CategoryName = table.Column<string>("nvarchar(500)", maxLength: 500, nullable: false),
                ParentId = table.Column<int>("int", nullable: false),
                UrlName = table.Column<string>("nvarchar(500)", maxLength: 500, nullable: true)
            },
            constraints: table => { table.PrimaryKey("Pk_Category_Id", x => x.Id); });

        migrationBuilder.CreateTable(
            "User",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                UserName = table.Column<string>("nvarchar(max)", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false, defaultValueSql: "((1))"),
                Role = table.Column<int>("int", nullable: false),
                PasswordHash = table.Column<byte[]>("varbinary(max)", nullable: false),
                PasswordSalt = table.Column<byte[]>("varbinary(max)", nullable: false),
                RefreshToken = table.Column<string>("nvarchar(max)", nullable: false),
                TokenCreated = table.Column<DateTime>("datetime2", nullable: false),
                TokenExpires = table.Column<DateTime>("datetime2", nullable: false)
            },
            constraints: table => { table.PrimaryKey("Pk_User_Id", x => x.Id); });

        migrationBuilder.CreateTable(
            "ArticleContent",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                ArticleId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Content = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("Pk_ArticleContent_Id", x => x.Id);
                table.ForeignKey(
                    "Fk_ArticleContent_ArticleId",
                    x => x.ArticleId,
                    "Article",
                    "Id");
            });

        migrationBuilder.CreateIndex(
            "Nidx_Article_ArticleName",
            "Article",
            "ArticleName");

        migrationBuilder.CreateIndex(
            "UQ__ArticleC__9C6270E936846DA0",
            "ArticleContent",
            "ArticleId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "ArticleContent");

        migrationBuilder.DropTable(
            "Category");

        migrationBuilder.DropTable(
            "User");

        migrationBuilder.DropTable(
            "Article");
    }
}