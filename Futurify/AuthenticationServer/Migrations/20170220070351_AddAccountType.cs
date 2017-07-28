using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using JobHop.Common.Enums;

namespace AuthenticationServer.Migrations
{
    public partial class AddAccountType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "Account",
                nullable: false,
                defaultValue: AccountType.Jobseeker);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "Account");
        }
    }
}
