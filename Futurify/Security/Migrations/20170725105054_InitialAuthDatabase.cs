using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Security.Migrations
{
    public partial class InitialAuthDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedByAccountId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailVerified = table.Column<bool>(nullable: true),
                    IsSystemAdmin = table.Column<bool>(nullable: true),
                    LockByAccountId = table.Column<int>(nullable: true),
                    LockUntil = table.Column<DateTime>(nullable: true),
                    Locked = table.Column<bool>(nullable: true),
                    LockedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    ModifiedByAccountId = table.Column<int>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberVerified = table.Column<bool>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_Account_CreatedByAccountId",
                        column: x => x.CreatedByAccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Account_LockByAccountId",
                        column: x => x.LockByAccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Account_ModifiedByAccountId",
                        column: x => x.ModifiedByAccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    PermissionId = table.Column<string>(nullable: false),
                    Display = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    Checked = table.Column<bool>(nullable: true),
                    ExpiredAt = table.Column<DateTime>(nullable: false),
                    Purpose = table.Column<int>(nullable: false),
                    Resend = table.Column<int>(nullable: true),
                    Retry = table.Column<int>(nullable: true),
                    SetEmail = table.Column<string>(nullable: true),
                    SetPhoneNumber = table.Column<string>(nullable: true),
                    VerifyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationCode_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountPermission",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPermission", x => new { x.AccountId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_AccountPermission_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountRole",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRole", x => new { x.AccountId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AccountRole_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreatedByAccountId",
                table: "Account",
                column: "CreatedByAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_LockByAccountId",
                table: "Account",
                column: "LockByAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_ModifiedByAccountId",
                table: "Account",
                column: "ModifiedByAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPermission_PermissionId",
                table: "AccountPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_RoleId",
                table: "AccountRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCode_AccountId",
                table: "VerificationCode",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPermission");

            migrationBuilder.DropTable(
                name: "AccountRole");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "VerificationCode");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
