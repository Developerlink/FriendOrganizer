using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendOrganizerDataAccessLibrary.Migrations
{
    public partial class AddedMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meeting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meeting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendMeeting",
                columns: table => new
                {
                    FriendsId = table.Column<int>(type: "int", nullable: false),
                    MeetingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendMeeting", x => new { x.FriendsId, x.MeetingsId });
                    table.ForeignKey(
                        name: "FK_FriendMeeting_Friend_FriendsId",
                        column: x => x.FriendsId,
                        principalTable: "Friend",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendMeeting_Meeting_MeetingsId",
                        column: x => x.MeetingsId,
                        principalTable: "Meeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendMeeting_MeetingsId",
                table: "FriendMeeting",
                column: "MeetingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendMeeting");

            migrationBuilder.DropTable(
                name: "Meeting");
        }
    }
}
