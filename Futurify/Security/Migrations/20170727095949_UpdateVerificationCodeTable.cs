using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Security.Migrations
{
    public partial class UpdateVerificationCodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checked",
                table: "VerificationCode");

            migrationBuilder.DropColumn(
                name: "Resend",
                table: "VerificationCode");

            migrationBuilder.DropColumn(
                name: "Retry",
                table: "VerificationCode");

            migrationBuilder.DropColumn(
                name: "SetEmail",
                table: "VerificationCode");

            migrationBuilder.RenameColumn(
                name: "VerifyCode",
                table: "VerificationCode",
                newName: "CodeReceiver");

            migrationBuilder.RenameColumn(
                name: "SetPhoneNumber",
                table: "VerificationCode",
                newName: "Code");

            migrationBuilder.AddColumn<int>(
                name: "CheckFailedCounter",
                table: "VerificationCode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VerificationCode",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ReceiverType",
                table: "VerificationCode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SendCounter",
                table: "VerificationCode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Used",
                table: "VerificationCode",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckFailedCounter",
                table: "VerificationCode");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VerificationCode");

            migrationBuilder.DropColumn(
                name: "ReceiverType",
                table: "VerificationCode");

            migrationBuilder.DropColumn(
                name: "SendCounter",
                table: "VerificationCode");

            migrationBuilder.DropColumn(
                name: "Used",
                table: "VerificationCode");

            migrationBuilder.RenameColumn(
                name: "CodeReceiver",
                table: "VerificationCode",
                newName: "VerifyCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "VerificationCode",
                newName: "SetPhoneNumber");

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "VerificationCode",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Resend",
                table: "VerificationCode",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Retry",
                table: "VerificationCode",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SetEmail",
                table: "VerificationCode",
                nullable: true);
        }
    }
}
