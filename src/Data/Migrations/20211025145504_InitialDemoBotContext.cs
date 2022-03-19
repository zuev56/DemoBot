using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoBot.Data.Migrations
{
    public partial class InitialDemoBotContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Script = table.Column<string>(type: "TEXT", maxLength: 5000, nullable: false),
                    DefaultArgs = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Group = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessengerInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessengerInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Permissions = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ChatTypeId = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    RawData = table.Column<string>(type: "TEXT", nullable: false),
                    RawDataHash = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RawDataHistory = table.Column<string>(type: "TEXT", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_ChatTypes_ChatTypeId",
                        column: x => x.ChatTypeId,
                        principalTable: "ChatTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserRoleId = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    IsBot = table.Column<bool>(type: "INTEGER", nullable: false),
                    RawData = table.Column<string>(type: "TEXT", nullable: false),
                    RawDataHash = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RawDataHistory = table.Column<string>(type: "TEXT", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplyToMessageId = table.Column<int>(type: "INTEGER", nullable: true),
                    MessengerId = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    MessageTypeId = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChatId = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    RawData = table.Column<string>(type: "TEXT", nullable: false),
                    RawDataHash = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RawDataHistory = table.Column<string>(type: "TEXT", nullable: true),
                    IsSucceed = table.Column<bool>(type: "INTEGER", nullable: false),
                    FailsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    FailDescription = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Messages_ReplyToMessageId",
                        column: x => x.ReplyToMessageId,
                        principalTable: "Messages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_MessageTypes_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalTable: "MessageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_MessengerInfos_MessengerId",
                        column: x => x.MessengerId,
                        principalTable: "MessengerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ChatTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "CHANNEL", "Channel" });

            migrationBuilder.InsertData(
                table: "ChatTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "GROUP", "Group" });

            migrationBuilder.InsertData(
                table: "ChatTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "PRIVATE", "Private" });

            migrationBuilder.InsertData(
                table: "ChatTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "UNDEFINED", "Undefined" });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "DefaultArgs", "Description", "Group", "Script" },
                values: new object[] { "/help", "<UserRoleId>", "Получение справки по доступным функциям", "userCmdGroup", "SELECT 'Not implemented for SQLite :('" });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "DefaultArgs", "Description", "Group", "Script" },
                values: new object[] { "/nulltest", null, "Тестовый запрос к боту. Возвращает NULL", "moderatorCmdGroup", "SELECT null" });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "DefaultArgs", "Description", "Group", "Script" },
                values: new object[] { "/sqlquery", "select 'Pass your query as a parameter in double quotes'", "SQL-запрос", "adminCmdGroup", "SELECT 'Not implemented for SQLite :('" });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "DefaultArgs", "Description", "Group", "Script" },
                values: new object[] { "/test", null, "Тестовый запрос к боту. Возвращает ''Test''", "moderatorCmdGroup", "SELECT 'Test'" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "AUD", "Audio" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "CNT", "Contact" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "DOC", "Document" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "LOC", "Location" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "OTH", "Other" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "PHT", "Photo" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "SRV", "Service message" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "STK", "Sticker" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "TXT", "Text" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "UKN", "Unknown" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "VID", "Video" });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "VOI", "Voice" });

            migrationBuilder.InsertData(
                table: "MessengerInfos",
                columns: new[] { "Id", "Name" },
                values: new object[] { "DC", "Discord" });

            migrationBuilder.InsertData(
                table: "MessengerInfos",
                columns: new[] { "Id", "Name" },
                values: new object[] { "FB", "Facebook" });

            migrationBuilder.InsertData(
                table: "MessengerInfos",
                columns: new[] { "Id", "Name" },
                values: new object[] { "SK", "Skype" });

            migrationBuilder.InsertData(
                table: "MessengerInfos",
                columns: new[] { "Id", "Name" },
                values: new object[] { "TG", "Telegram" });

            migrationBuilder.InsertData(
                table: "MessengerInfos",
                columns: new[] { "Id", "Name" },
                values: new object[] { "VK", "Вконтакте" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Name", "Permissions" },
                values: new object[] { "ADMIN", "Administrator", "[ \"adminCmdGroup\", \"moderatorCmdGroup\", \"userCmdGroup\" ]" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Name", "Permissions" },
                values: new object[] { "MODERATOR", "Moderator", "[ \"moderatorCmdGroup\", \"userCmdGroup\" ]" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Name", "Permissions" },
                values: new object[] { "OWNER", "Owner", "[ \"All\" ]" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Name", "Permissions" },
                values: new object[] { "USER", "User", "[ \"userCmdGroup\" ]" });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "Id", "ChatTypeId", "Description", "InsertDate", "Name", "RawData", "RawDataHash", "RawDataHistory" },
                values: new object[] { -1, "PRIVATE", "IntegrationTestChat", new DateTime(2021, 10, 25, 14, 55, 4, 361, DateTimeKind.Utc).AddTicks(9681), "IntegrationTestChat", "{ \"test\": \"test\" }", "-1063294487", null });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "Id", "ChatTypeId", "Description", "InsertDate", "Name", "RawData", "RawDataHash", "RawDataHistory" },
                values: new object[] { 1, "PRIVATE", null, new DateTime(2021, 10, 25, 14, 55, 4, 361, DateTimeKind.Utc).AddTicks(9683), "zuev56", "{ \"Id\": 210281448 }", "-1063294487", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "InsertDate", "IsBot", "Name", "RawData", "RawDataHash", "RawDataHistory", "UserRoleId" },
                values: new object[] { -10, "for exported message reading", new DateTime(2021, 10, 25, 14, 55, 4, 361, DateTimeKind.Utc).AddTicks(9714), false, "Unknown", "{ \"test\": \"test\" }", "-1063294487", null, "USER" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "InsertDate", "IsBot", "Name", "RawData", "RawDataHash", "RawDataHistory", "UserRoleId" },
                values: new object[] { -1, "IntegrationTest", new DateTime(2021, 10, 25, 14, 55, 4, 361, DateTimeKind.Utc).AddTicks(9716), false, "IntegrationTestUser", "{ \"test\": \"test\" }", "-1063294487", null, "USER" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "InsertDate", "IsBot", "Name", "RawData", "RawDataHash", "RawDataHistory", "UserRoleId" },
                values: new object[] { 1, "Сергей Зуев", new DateTime(2021, 10, 25, 14, 55, 4, 361, DateTimeKind.Utc).AddTicks(9717), false, "zuev56", "{ \"Id\": 210281448 }", "-1063294487", null, "OWNER" });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ChatTypeId",
                table: "Chats",
                column: "ChatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageTypeId",
                table: "Messages",
                column: "MessageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessengerId",
                table: "Messages",
                column: "MessengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReplyToMessageId",
                table: "Messages",
                column: "ReplyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "MessageTypes");

            migrationBuilder.DropTable(
                name: "MessengerInfos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ChatTypes");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
