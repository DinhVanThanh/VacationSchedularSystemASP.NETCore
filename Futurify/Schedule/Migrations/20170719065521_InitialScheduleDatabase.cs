using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Schedule.Migrations
{
    public partial class InitialScheduleDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacations",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    End = table.Column<DateTime>(nullable: true),
                    IsAllDay = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    OwnerId = table.Column<int>(nullable: true),
                    Start = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacations", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vacations");
        }
    }
}
