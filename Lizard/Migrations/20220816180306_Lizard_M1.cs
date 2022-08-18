using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lizard.Migrations
{
    public partial class Lizard_M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "lizard");

            migrationBuilder.CreateTable(
                name: "Source",
                schema: "lizard",
                columns: table => new
                {
                    SourceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.SourceID);
                });

            migrationBuilder.CreateTable(
                name: "LogEntry",
                schema: "lizard",
                columns: table => new
                {
                    LogEntryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "-9223372036854775808, 1"),
                    SHA256 = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    SourceID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntry", x => x.LogEntryID);
                    table.ForeignKey(
                        name: "FK_LogEntry_Source_SourceID",
                        column: x => x.SourceID,
                        principalSchema: "lizard",
                        principalTable: "Source",
                        principalColumn: "SourceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLogEntry",
                schema: "lizard",
                columns: table => new
                {
                    LogEntryID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogEntry", x => x.LogEntryID);
                    table.ForeignKey(
                        name: "FK_ExceptionLogEntry_LogEntry_LogEntryID",
                        column: x => x.LogEntryID,
                        principalSchema: "lizard",
                        principalTable: "LogEntry",
                        principalColumn: "LogEntryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HttpRequestLogEntry",
                schema: "lizard",
                columns: table => new
                {
                    LogEntryID = table.Column<long>(type: "bigint", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Headers = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpRequestLogEntry", x => x.LogEntryID);
                    table.ForeignKey(
                        name: "FK_HttpRequestLogEntry_LogEntry_LogEntryID",
                        column: x => x.LogEntryID,
                        principalSchema: "lizard",
                        principalTable: "LogEntry",
                        principalColumn: "LogEntryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Occurrence",
                schema: "lizard",
                columns: table => new
                {
                    OccurrenceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogEntryID = table.Column<long>(type: "bigint", nullable: false),
                    Occurred = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Written = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occurrence", x => x.OccurrenceID);
                    table.ForeignKey(
                        name: "FK_Occurrence_LogEntry_LogEntryID",
                        column: x => x.LogEntryID,
                        principalSchema: "lizard",
                        principalTable: "LogEntry",
                        principalColumn: "LogEntryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InnerExceptionLogEntry",
                schema: "lizard",
                columns: table => new
                {
                    InnerExceptionLogEntryID = table.Column<long>(type: "bigint", nullable: false),
                    OuterExceptionLogEntryID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InnerExceptionLogEntry", x => x.InnerExceptionLogEntryID);
                    table.ForeignKey(
                        name: "FK_InnerExceptionLogEntry_ExceptionLogEntry_InnerExceptionLogEntryID",
                        column: x => x.InnerExceptionLogEntryID,
                        principalSchema: "lizard",
                        principalTable: "ExceptionLogEntry",
                        principalColumn: "LogEntryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InnerExceptionLogEntry_ExceptionLogEntry_OuterExceptionLogEntryID",
                        column: x => x.OuterExceptionLogEntryID,
                        principalSchema: "lizard",
                        principalTable: "ExceptionLogEntry",
                        principalColumn: "LogEntryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StackTrace",
                schema: "lizard",
                columns: table => new
                {
                    ExceptionLogEntryID = table.Column<long>(type: "bigint", nullable: false),
                    TargetSite = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackTrace", x => x.ExceptionLogEntryID);
                    table.ForeignKey(
                        name: "FK_StackTrace_ExceptionLogEntry_ExceptionLogEntryID",
                        column: x => x.ExceptionLogEntryID,
                        principalSchema: "lizard",
                        principalTable: "ExceptionLogEntry",
                        principalColumn: "LogEntryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HttpResponseLogEntry",
                schema: "lizard",
                columns: table => new
                {
                    LogEntryID = table.Column<long>(type: "bigint", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Headers = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpResponseLogEntry", x => x.LogEntryID);
                    table.ForeignKey(
                        name: "FK_HttpResponseLogEntry_HttpRequestLogEntry_LogEntryID",
                        column: x => x.LogEntryID,
                        principalSchema: "lizard",
                        principalTable: "HttpRequestLogEntry",
                        principalColumn: "LogEntryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InnerExceptionLogEntry_OuterExceptionLogEntryID",
                schema: "lizard",
                table: "InnerExceptionLogEntry",
                column: "OuterExceptionLogEntryID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogEntry_SourceID",
                schema: "lizard",
                table: "LogEntry",
                column: "SourceID");

            migrationBuilder.CreateIndex(
                name: "IX_Occurrence_LogEntryID",
                schema: "lizard",
                table: "Occurrence",
                column: "LogEntryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HttpResponseLogEntry",
                schema: "lizard");

            migrationBuilder.DropTable(
                name: "InnerExceptionLogEntry",
                schema: "lizard");

            migrationBuilder.DropTable(
                name: "Occurrence",
                schema: "lizard");

            migrationBuilder.DropTable(
                name: "StackTrace",
                schema: "lizard");

            migrationBuilder.DropTable(
                name: "HttpRequestLogEntry",
                schema: "lizard");

            migrationBuilder.DropTable(
                name: "ExceptionLogEntry",
                schema: "lizard");

            migrationBuilder.DropTable(
                name: "LogEntry",
                schema: "lizard");

            migrationBuilder.DropTable(
                name: "Source",
                schema: "lizard");
        }
    }
}
