using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationServer.Migrations
{
    public partial class SecureForVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Resend",
                table: "VerificationCode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Retry",
                table: "VerificationCode",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resend",
                table: "VerificationCode");

            migrationBuilder.DropColumn(
                name: "Retry",
                table: "VerificationCode");
        }
    }
}
