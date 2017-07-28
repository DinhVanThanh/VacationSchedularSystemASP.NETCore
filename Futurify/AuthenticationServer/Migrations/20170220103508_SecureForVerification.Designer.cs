using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AuthenticationServer.Models;

namespace AuthenticationServer.Migrations
{
    [DbContext(typeof(AuthContext))]
    [Migration("20170220103508_SecureForVerification")]
    partial class SecureForVerification
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AuthenticationService.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountType");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<int?>("CreatedById");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailVerified");

                    b.Property<bool>("IsSystemAdmin");

                    b.Property<int?>("LockById");

                    b.Property<DateTime?>("LockUntil");

                    b.Property<bool>("Locked");

                    b.Property<DateTime?>("LockedAt");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<int?>("ModifiedById");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberVerified");

                    b.Property<string>("SecurityStamp")
                        .IsRequired();

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LockById");

                    b.HasIndex("ModifiedById");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("AuthenticationService.Models.AccountPermission", b =>
                {
                    b.Property<int>("AccountId");

                    b.Property<string>("PermissionId");

                    b.HasKey("AccountId", "PermissionId");

                    b.HasIndex("AccountId");

                    b.HasIndex("PermissionId");

                    b.ToTable("AccountPermission");
                });

            modelBuilder.Entity("AuthenticationService.Models.AccountRole", b =>
                {
                    b.Property<int>("AccountId");

                    b.Property<int>("RoleId");

                    b.HasKey("AccountId", "RoleId");

                    b.HasIndex("AccountId");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountRole");
                });

            modelBuilder.Entity("AuthenticationService.Models.ExternalProvider", b =>
                {
                    b.Property<string>("Provider");

                    b.Property<string>("ProviderKey");

                    b.Property<int?>("AccountId")
                        .IsRequired();

                    b.HasKey("Provider", "ProviderKey");

                    b.HasIndex("AccountId");

                    b.ToTable("ExternalProvider");
                });

            modelBuilder.Entity("AuthenticationService.Models.Permission", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Display");

                    b.HasKey("Id");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("AuthenticationService.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("AuthenticationService.Models.RolePermission", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<string>("PermissionId");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermission");
                });

            modelBuilder.Entity("AuthenticationService.Models.VerificationCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<bool>("Checked");

                    b.Property<DateTime>("ExpiredAt");

                    b.Property<int>("Purpose");

                    b.Property<int>("Resend");

                    b.Property<int>("Retry");

                    b.Property<string>("SetEmail");

                    b.Property<string>("SetPhoneNumber");

                    b.Property<string>("VerifyCode");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("VerificationCode");
                });

            modelBuilder.Entity("AuthenticationService.Models.Account", b =>
                {
                    b.HasOne("AuthenticationService.Models.Account", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("AuthenticationService.Models.Account", "LockBy")
                        .WithMany()
                        .HasForeignKey("LockById");

                    b.HasOne("AuthenticationService.Models.Account", "ModifiedBy")
                        .WithMany()
                        .HasForeignKey("ModifiedById");
                });

            modelBuilder.Entity("AuthenticationService.Models.AccountPermission", b =>
                {
                    b.HasOne("AuthenticationService.Models.Account", "Account")
                        .WithMany("AccountPermissions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuthenticationService.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthenticationService.Models.AccountRole", b =>
                {
                    b.HasOne("AuthenticationService.Models.Account", "Account")
                        .WithMany("AccountRoles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuthenticationService.Models.Role", "Role")
                        .WithMany("AccountRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthenticationService.Models.ExternalProvider", b =>
                {
                    b.HasOne("AuthenticationService.Models.Account", "Account")
                        .WithMany("ExternalLogins")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthenticationService.Models.RolePermission", b =>
                {
                    b.HasOne("AuthenticationService.Models.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuthenticationService.Models.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthenticationService.Models.VerificationCode", b =>
                {
                    b.HasOne("AuthenticationService.Models.Account", "Account")
                        .WithMany("VerificationCodes")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
