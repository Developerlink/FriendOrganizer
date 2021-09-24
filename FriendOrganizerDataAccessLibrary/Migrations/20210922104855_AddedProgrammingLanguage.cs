using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendOrganizerDataAccessLibrary.Migrations
{
    public partial class AddedProgrammingLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FavoriteLanguageId",
                table: "Friend",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProgrammingLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammingLanguage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friend_FavoriteLanguageId",
                table: "Friend",
                column: "FavoriteLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_ProgrammingLanguage_FavoriteLanguageId",
                table: "Friend",
                column: "FavoriteLanguageId",
                principalTable: "ProgrammingLanguage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_ProgrammingLanguage_FavoriteLanguageId",
                table: "Friend");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguage");

            migrationBuilder.DropIndex(
                name: "IX_Friend_FavoriteLanguageId",
                table: "Friend");

            migrationBuilder.DropColumn(
                name: "FavoriteLanguageId",
                table: "Friend");
        }
    }
}
