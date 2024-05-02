using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E2_Service.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ApplicationUser_ReceivingUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ApplicationUser_SendingUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ApplicationUser_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ApplicationUser_ManagerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ApplicationUser_ServicemanId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser");

            migrationBuilder.RenameTable(
                name: "ApplicationUser",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceivingUserId",
                table: "Messages",
                column: "ReceivingUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SendingUserId",
                table: "Messages",
                column: "SendingUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_ManagerId",
                table: "Orders",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Users_ServicemanId",
                table: "Services",
                column: "ServicemanId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceivingUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SendingUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_ManagerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Users_ServicemanId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "ApplicationUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ApplicationUser_ReceivingUserId",
                table: "Messages",
                column: "ReceivingUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ApplicationUser_SendingUserId",
                table: "Messages",
                column: "SendingUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ApplicationUser_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ApplicationUser_ManagerId",
                table: "Orders",
                column: "ManagerId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ApplicationUser_ServicemanId",
                table: "Services",
                column: "ServicemanId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
