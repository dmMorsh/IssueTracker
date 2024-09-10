using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ApplicationUserChatEntity",
                newName: "ApplicationUserChat");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChatEntity_chats_ChatsId",
                table: "ApplicationUserChat");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_tickets_TicketId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_executionList_AspNetUsers_UserId",
                table: "executionList");

            migrationBuilder.DropForeignKey(
                name: "FK_executionList_tickets_TicketId",
                table: "executionList");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AspNetUsers_CreatorId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AspNetUsers_ExecutorId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_spaces_SpaceId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_watchList_AspNetUsers_UserId",
                table: "watchList");

            migrationBuilder.DropForeignKey(
                name: "FK_watchList_tickets_TicketId",
                table: "watchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_watchList",
                table: "watchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tickets",
                table: "tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_spaces",
                table: "spaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_messages",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_executionList",
                table: "executionList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comments",
                table: "comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chats",
                table: "chats");

            migrationBuilder.RenameTable(
                name: "watchList",
                newName: "WatchList");

            migrationBuilder.RenameTable(
                name: "tickets",
                newName: "Tickets");

            migrationBuilder.RenameTable(
                name: "spaces",
                newName: "Spaces");

            migrationBuilder.RenameTable(
                name: "messages",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "executionList",
                newName: "ExecutionList");

            migrationBuilder.RenameTable(
                name: "comments",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "chats",
                newName: "Chats");

            migrationBuilder.RenameIndex(
                name: "IX_watchList_TicketId",
                table: "WatchList",
                newName: "IX_WatchList_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_SpaceId",
                table: "Tickets",
                newName: "IX_Tickets_SpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_ExecutorId",
                table: "Tickets",
                newName: "IX_Tickets_ExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_CreatorId",
                table: "Tickets",
                newName: "IX_Tickets_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_executionList_TicketId",
                table: "ExecutionList",
                newName: "IX_ExecutionList_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_TicketId",
                table: "Comments",
                newName: "IX_Comments_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList",
                columns: new[] { "UserId", "TicketId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spaces",
                table: "Spaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExecutionList",
                table: "ExecutionList",
                columns: new[] { "UserId", "TicketId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChat_Chats_ChatsId",
                table: "ApplicationUserChat",
                column: "ChatsId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionList_AspNetUsers_UserId",
                table: "ExecutionList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionList_Tickets_TicketId",
                table: "ExecutionList",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_CreatorId",
                table: "Tickets",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_ExecutorId",
                table: "Tickets",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Spaces_SpaceId",
                table: "Tickets",
                column: "SpaceId",
                principalTable: "Spaces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_AspNetUsers_UserId",
                table: "WatchList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_Tickets_TicketId",
                table: "WatchList",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ApplicationUserChat",
                newName: "ApplicationUserChatEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChat_Chats_ChatsId",
                table: "ApplicationUserChat");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionList_AspNetUsers_UserId",
                table: "ExecutionList");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionList_Tickets_TicketId",
                table: "ExecutionList");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_CreatorId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_ExecutorId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Spaces_SpaceId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_AspNetUsers_UserId",
                table: "WatchList");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_Tickets_TicketId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spaces",
                table: "Spaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExecutionList",
                table: "ExecutionList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "WatchList",
                newName: "watchList");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "tickets");

            migrationBuilder.RenameTable(
                name: "Spaces",
                newName: "spaces");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "messages");

            migrationBuilder.RenameTable(
                name: "ExecutionList",
                newName: "executionList");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "comments");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "chats");

            migrationBuilder.RenameIndex(
                name: "IX_WatchList_TicketId",
                table: "watchList",
                newName: "IX_watchList_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_SpaceId",
                table: "tickets",
                newName: "IX_tickets_SpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_ExecutorId",
                table: "tickets",
                newName: "IX_tickets_ExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_CreatorId",
                table: "tickets",
                newName: "IX_tickets_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_ExecutionList_TicketId",
                table: "executionList",
                newName: "IX_executionList_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_TicketId",
                table: "comments",
                newName: "IX_comments_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_watchList",
                table: "watchList",
                columns: new[] { "UserId", "TicketId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tickets",
                table: "tickets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_spaces",
                table: "spaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_messages",
                table: "messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_executionList",
                table: "executionList",
                columns: new[] { "UserId", "TicketId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_comments",
                table: "comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chats",
                table: "chats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChatEntity_chats_ChatsId",
                table: "ApplicationUserChat",
                column: "ChatsId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_tickets_TicketId",
                table: "comments",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_executionList_AspNetUsers_UserId",
                table: "executionList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_executionList_tickets_TicketId",
                table: "executionList",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AspNetUsers_CreatorId",
                table: "tickets",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AspNetUsers_ExecutorId",
                table: "tickets",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_spaces_SpaceId",
                table: "tickets",
                column: "SpaceId",
                principalTable: "spaces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_watchList_AspNetUsers_UserId",
                table: "watchList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_watchList_tickets_TicketId",
                table: "watchList",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
