using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Security.Models;

namespace Security.Migrations
{
    [DbContext(typeof(AuthContext))]
    [Migration("20170727095949_UpdateVerificationCodeTable")]
    partial class UpdateVerificationCodeTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Security.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<int?>("CreatedByAccountId");

                    b.Property<string>("Email");

                    b.Property<bool?>("EmailVerified");

                    b.Property<bool?>("IsSystemAdmin");

                    b.Property<int?>("LockByAccountId");

                    b.Property<DateTime?>("LockUntil");

                    b.Property<bool?>("Locked");

                    b.Property<DateTime?>("LockedAt");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<int?>("ModifiedByAccountId");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool?>("PhoneNumberVerified");

                    b.Property<int?>("Status");

                    b.Property<int?>("Type");

                    b.Property<string>("UserName");

                    b.HasKey("AccountId");

                    b.HasIndex("CreatedByAccountId");

                    b.HasIndex("LockByAccountId");

                    b.HasIndex("ModifiedByAccountId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Security.Models.AccountPermission", b =>
                {
                    b.Property<int>("AccountId");

                    b.Property<string>("PermissionId");

                    b.HasKey("AccountId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("AccountPermission");
                });

            modelBuilder.Entity("Security.Models.AccountRole", b =>
                {
                    b.Property<int>("AccountId");

                    b.Property<int>("RoleId");

                    b.HasKey("AccountId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountRole");
                });

            modelBuilder.Entity("Security.Models.Permission", b =>
                {
                    b.Property<string>("PermissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Display");

                    b.HasKey("PermissionId");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Security.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Security.Models.RolePermission", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<string>("PermissionId");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermission");
                });

            modelBuilder.Entity("Security.Models.VerificationCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<int>("CheckFailedCounter");

                    b.Property<string>("Code");

                    b.Property<string>("CodeReceiver");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("ExpiredAt");

                    b.Property<int>("Purpose");

                    b.Property<int>("ReceiverType");

                    b.Property<int>("SendCounter");

                    b.Property<bool>("Used");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("VerificationCode");
                });

            modelBuilder.Entity("Security.Models.Account", b =>
                {
                    b.HasOne("Security.Models.Account", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByAccountId");

                    b.HasOne("Security.Models.Account", "LockBy")
                        .WithMany()
                        .HasForeignKey("LockByAccountId");

                    b.HasOne("Security.Models.Account", "ModifiedBy")
                        .WithMany()
                        .HasForeignKey("ModifiedByAccountId");
                });

            modelBuilder.Entity("Security.Models.AccountPermission", b =>
                {
                    b.HasOne("Security.Models.Account", "Account")
                        .WithMany("AccountPermissions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Security.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Security.Models.AccountRole", b =>
                {
                    b.HasOne("Security.Models.Account", "Account")
                        .WithMany("AccountRoles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Security.Models.Role", "Role")
                        .WithMany("AccountRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Security.Models.RolePermission", b =>
                {
                    b.HasOne("Security.Models.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Security.Models.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Security.Models.VerificationCode", b =>
                {
                    b.HasOne("Security.Models.Account", "Account")
                        .WithMany("VerificationCodes")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
