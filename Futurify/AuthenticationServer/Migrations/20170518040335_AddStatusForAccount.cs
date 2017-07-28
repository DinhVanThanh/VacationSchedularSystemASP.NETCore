using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationServer.Migrations
{
    public partial class AddStatusForAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_RolePermission_RoleId",
            //    table: "RolePermission");

            //migrationBuilder.DropIndex(
            //    name: "IX_AccountRole_AccountId",
            //    table: "AccountRole");

            //migrationBuilder.DropIndex(
            //    name: "IX_AccountPermission_AccountId",
            //    table: "AccountPermission");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Account",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Account");

            //migrationBuilder.CreateIndex(
            //    name: "IX_RolePermission_RoleId",
            //    table: "RolePermission",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AccountRole_AccountId",
            //    table: "AccountRole",
            //    column: "AccountId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AccountPermission_AccountId",
            //    table: "AccountPermission",
            //    column: "AccountId");
        }
    }
}
