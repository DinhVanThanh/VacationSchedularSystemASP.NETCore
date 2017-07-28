using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuthenticationServer.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailVerified = table.Column<bool>(nullable: false),
                    IsSystemAdmin = table.Column<bool>(nullable: false),
                    LockById = table.Column<int>(nullable: true),
                    LockUntil = table.Column<DateTime>(nullable: true),
                    Locked = table.Column<bool>(nullable: false),
                    LockedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    ModifiedById = table.Column<int>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberVerified = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Account_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Account_LockById",
                        column: x => x.LockById,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Account_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Display = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalProvider",
                columns: table => new
                {
                    Provider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalProvider", x => new { x.Provider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_ExternalProvider_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    Checked = table.Column<bool>(nullable: false),
                    ExpiredAt = table.Column<DateTime>(nullable: false),
                    Purpose = table.Column<int>(nullable: false),
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
                        principalColumn: "Id",
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreatedById",
                table: "Account",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Account_LockById",
                table: "Account",
                column: "LockById");

            migrationBuilder.CreateIndex(
                name: "IX_Account_ModifiedById",
                table: "Account",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPermission_AccountId",
                table: "AccountPermission",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPermission_PermissionId",
                table: "AccountPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_AccountId",
                table: "AccountRole",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_RoleId",
                table: "AccountRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalProvider_AccountId",
                table: "ExternalProvider",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

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
                name: "ExternalProvider");

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
