using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationServer.Migrations
{
    public partial class SetAllAccountToActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //set all current account to active
            migrationBuilder.Sql("Update Account set Status = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //set all current account to pending
            migrationBuilder.Sql("Update Account set Status = 0");
        }
    }
}
