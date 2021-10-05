using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendOrganizerDataAccessLibrary.Migrations
{
    public partial class AddedRowVersionToFriend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Friend",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Friend");
        }
    }
}
