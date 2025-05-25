using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");

            migrationBuilder.CreateTable(
                name: "CATEGORY_TB",
                columns: table => new
                {
                    CATEGORY_ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    NAME = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORY_TB", x => x.CATEGORY_ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLE_TB",
                columns: table => new
                {
                    ROLE_ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    NAME = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE_TB", x => x.ROLE_ID);
                });

            migrationBuilder.CreateTable(
                name: "GAME_TB",
                columns: table => new
                {
                    GAME_ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    TITLE = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IMAGE_URL = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ACTIVE = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAME_TB", x => x.GAME_ID);
                    table.ForeignKey(
                        name: "FK_GAME_TB_CATEGORY_TB_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CATEGORY_TB",
                        principalColumn: "CATEGORY_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USER_TB",
                columns: table => new
                {
                    USER_ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    USERNAME = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EMAIL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "text", nullable: false),
                    FIRSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LASTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ACTIVE = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_TB", x => x.USER_ID);
                    table.ForeignKey(
                        name: "FK_USER_TB_ROLE_TB_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ROLE_TB",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LIBRARY_TB",
                columns: table => new
                {
                    LIBRARY_ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    DT_PURCHASED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    USER_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    GAME_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIBRARY_TB", x => x.LIBRARY_ID);
                    table.ForeignKey(
                        name: "FK_LIBRARY_TB_GAME_TB_GAME_ID",
                        column: x => x.GAME_ID,
                        principalTable: "GAME_TB",
                        principalColumn: "GAME_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LIBRARY_TB_USER_TB_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USER_TB",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SALE_TB",
                columns: table => new
                {
                    SALE_ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DISCOUNT_PERCENTAGE = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    DT_START_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DT_END_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ACTIVE = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALE_TB", x => x.SALE_ID);
                    table.ForeignKey(
                        name: "FK_SALE_TB_GAME_TB_GameId",
                        column: x => x.GameId,
                        principalTable: "GAME_TB",
                        principalColumn: "GAME_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SALE_TB_USER_TB_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "USER_TB",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SESSION_TB",
                columns: table => new
                {
                    SESSION_ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    TOKEN = table.Column<string>(type: "text", nullable: false),
                    DT_EXPIRES_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ACTIVE = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION_TB", x => x.SESSION_ID);
                    table.ForeignKey(
                        name: "FK_SESSION_TB_USER_TB_UserId",
                        column: x => x.UserId,
                        principalTable: "USER_TB",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORY_TB_NAME",
                table: "CATEGORY_TB",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GAME_TB_CategoryId",
                table: "GAME_TB",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LIBRARY_TB_GAME_ID",
                table: "LIBRARY_TB",
                column: "GAME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LIBRARY_TB_USER_ID_GAME_ID",
                table: "LIBRARY_TB",
                columns: new[] { "USER_ID", "GAME_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROLE_TB_NAME",
                table: "ROLE_TB",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SALE_TB_CreatedByUserId",
                table: "SALE_TB",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SALE_TB_GameId",
                table: "SALE_TB",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_SESSION_TB_TOKEN",
                table: "SESSION_TB",
                column: "TOKEN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SESSION_TB_UserId",
                table: "SESSION_TB",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_TB_EMAIL",
                table: "USER_TB",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_TB_RoleId",
                table: "USER_TB",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_TB_USERNAME",
                table: "USER_TB",
                column: "USERNAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LIBRARY_TB");

            migrationBuilder.DropTable(
                name: "SALE_TB");

            migrationBuilder.DropTable(
                name: "SESSION_TB");

            migrationBuilder.DropTable(
                name: "GAME_TB");

            migrationBuilder.DropTable(
                name: "USER_TB");

            migrationBuilder.DropTable(
                name: "CATEGORY_TB");

            migrationBuilder.DropTable(
                name: "ROLE_TB");
        }
    }
}
