using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Articles.RestApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userinfo",
                schema: "articles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userinfo", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userinfo_Username",
                schema: "articles",
                table: "userinfo",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userinfo",
                schema: "articles");
        }
    }
}
