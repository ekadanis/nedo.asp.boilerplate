using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nedo.asp.boilerplate.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbs000_project",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    project_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    project_description = table.Column<string>(type: "text", nullable: true),
                    project_startdate = table.Column<DateOnly>(type: "date", nullable: true),
                    project_enddate = table.Column<DateOnly>(type: "date", nullable: true),
                    project_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    project_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    project_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    project_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    project_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    project_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    project_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    project_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_project", x => x.project_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_document",
                columns: table => new
                {
                    document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    document_filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    document_filepath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    document_filesize = table.Column<long>(type: "bigint", nullable: false),
                    document_mimetype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    document_projectid = table.Column<Guid>(type: "uuid", nullable: false),
                    document_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    document_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    document_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    document_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    document_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    document_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    document_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_document", x => x.document_id);
                    table.ForeignKey(
                        name: "FK_dbs000_document_dbs000_project_document_projectid",
                        column: x => x.document_projectid,
                        principalTable: "dbs000_project",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dbs000_document_createddate",
                table: "dbs000_document",
                column: "document_createddate");

            migrationBuilder.CreateIndex(
                name: "ix_dbs000_document_projectid",
                table: "dbs000_document",
                column: "document_projectid");

            migrationBuilder.CreateIndex(
                name: "ix_dbs000_project_createddate",
                table: "dbs000_project",
                column: "project_createddate");

            migrationBuilder.CreateIndex(
                name: "uq_dbs000_project_code",
                table: "dbs000_project",
                column: "project_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbs000_document");

            migrationBuilder.DropTable(
                name: "dbs000_project");
        }
    }
}
