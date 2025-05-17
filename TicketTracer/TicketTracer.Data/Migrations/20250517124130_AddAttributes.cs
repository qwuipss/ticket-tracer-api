using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketTracer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_boards_project_id",
                table: "boards");

            migrationBuilder.DropIndex(
                name: "IX_boards_title",
                table: "boards");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "attributes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    board_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attributes", x => x.id);
                    table.ForeignKey(
                        name: "FK_attributes_boards_board_id",
                        column: x => x.board_id,
                        principalTable: "boards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attribute_values",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    attribute_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attribute_values", x => x.id);
                    table.ForeignKey(
                        name: "FK_attribute_values_attributes_attribute_id",
                        column: x => x.attribute_id,
                        principalTable: "attributes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attribute_values_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_boards_project_id_title",
                table: "boards",
                columns: new[] { "project_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attribute_values_attribute_id",
                table: "attribute_values",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "IX_attribute_values_ticket_id",
                table: "attribute_values",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "IX_attributes_board_id_name",
                table: "attributes",
                columns: new[] { "board_id", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attribute_values");

            migrationBuilder.DropTable(
                name: "attributes");

            migrationBuilder.DropIndex(
                name: "IX_boards_project_id_title",
                table: "boards");

            migrationBuilder.DropColumn(
                name: "type",
                table: "tickets");

            migrationBuilder.CreateIndex(
                name: "IX_boards_project_id",
                table: "boards",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_boards_title",
                table: "boards",
                column: "title",
                unique: true);
        }
    }
}
