using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Dal.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChatEntity_chats_Chatsid",
                table: "ApplicationUserChatEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_tickets_ticketId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AspNetUsers_creatorId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AspNetUsers_executorId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_spaces_spaceid",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "updatedDate",
                table: "tickets",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "tickets",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "tickets",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "spaceid",
                table: "tickets",
                newName: "SpaceId");

            migrationBuilder.RenameColumn(
                name: "priority",
                table: "tickets",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "issueType",
                table: "tickets",
                newName: "IssueType");

            migrationBuilder.RenameColumn(
                name: "executorId",
                table: "tickets",
                newName: "ExecutorId");

            migrationBuilder.RenameColumn(
                name: "dueDate",
                table: "tickets",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "tickets",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "creatorId",
                table: "tickets",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "createDate",
                table: "tickets",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tickets",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_spaceid",
                table: "tickets",
                newName: "IX_tickets_SpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_executorId",
                table: "tickets",
                newName: "IX_tickets_ExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_creatorId",
                table: "tickets",
                newName: "IX_tickets_CreatorId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "spaces",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "spaces",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "sender",
                table: "messages",
                newName: "Sender");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "messages",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "dateSent",
                table: "messages",
                newName: "DateSent");

            migrationBuilder.RenameColumn(
                name: "chatId",
                table: "messages",
                newName: "ChatId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "messages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ticketId",
                table: "comments",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "edited",
                table: "comments",
                newName: "Edited");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "comments",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "creator",
                table: "comments",
                newName: "Creator");

            migrationBuilder.RenameColumn(
                name: "createDate",
                table: "comments",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "comments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_comments_ticketId",
                table: "comments",
                newName: "IX_comments_TicketId");

            migrationBuilder.RenameColumn(
                name: "isPersonal",
                table: "chats",
                newName: "IsPersonal");

            migrationBuilder.RenameColumn(
                name: "info",
                table: "chats",
                newName: "Info");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "chats",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Chatsid",
                table: "ApplicationUserChatEntity",
                newName: "ChatsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChatEntity_chats_ChatsId",
                table: "ApplicationUserChatEntity",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChatEntity_chats_ChatsId",
                table: "ApplicationUserChatEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_tickets_TicketId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AspNetUsers_CreatorId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AspNetUsers_ExecutorId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_spaces_SpaceId",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "tickets",
                newName: "updatedDate");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "tickets",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "tickets",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "SpaceId",
                table: "tickets",
                newName: "spaceid");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "tickets",
                newName: "priority");

            migrationBuilder.RenameColumn(
                name: "IssueType",
                table: "tickets",
                newName: "issueType");

            migrationBuilder.RenameColumn(
                name: "ExecutorId",
                table: "tickets",
                newName: "executorId");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "tickets",
                newName: "dueDate");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "tickets",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "tickets",
                newName: "creatorId");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "tickets",
                newName: "createDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tickets",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_SpaceId",
                table: "tickets",
                newName: "IX_tickets_spaceid");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_ExecutorId",
                table: "tickets",
                newName: "IX_tickets_executorId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_CreatorId",
                table: "tickets",
                newName: "IX_tickets_creatorId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "spaces",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "spaces",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Sender",
                table: "messages",
                newName: "sender");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "messages",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "DateSent",
                table: "messages",
                newName: "dateSent");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "messages",
                newName: "chatId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "messages",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "comments",
                newName: "ticketId");

            migrationBuilder.RenameColumn(
                name: "Edited",
                table: "comments",
                newName: "edited");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "comments",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Creator",
                table: "comments",
                newName: "creator");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "comments",
                newName: "createDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "comments",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_comments_TicketId",
                table: "comments",
                newName: "IX_comments_ticketId");

            migrationBuilder.RenameColumn(
                name: "IsPersonal",
                table: "chats",
                newName: "isPersonal");

            migrationBuilder.RenameColumn(
                name: "Info",
                table: "chats",
                newName: "info");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "chats",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ChatsId",
                table: "ApplicationUserChatEntity",
                newName: "Chatsid");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChatEntity_chats_Chatsid",
                table: "ApplicationUserChatEntity",
                column: "Chatsid",
                principalTable: "chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_tickets_ticketId",
                table: "comments",
                column: "ticketId",
                principalTable: "tickets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AspNetUsers_creatorId",
                table: "tickets",
                column: "creatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AspNetUsers_executorId",
                table: "tickets",
                column: "executorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_spaces_spaceid",
                table: "tickets",
                column: "spaceid",
                principalTable: "spaces",
                principalColumn: "id");
        }
    }
}
