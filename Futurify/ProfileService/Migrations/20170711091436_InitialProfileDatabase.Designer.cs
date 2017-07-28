using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ProfileService.Model;

namespace ProfileService.Migrations
{
    [DbContext(typeof(ProfileContext))]
    [Migration("20170711091436_InitialProfileDatabase")]
    partial class InitialProfileDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProfileService.Model.Apartment", b =>
                {
                    b.Property<int>("ApartmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApartmentName");

                    b.HasKey("ApartmentId");

                    b.ToTable("Apartments");
                });

            modelBuilder.Entity("ProfileService.Model.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ApartmentId");

                    b.Property<string>("Avatar");

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int?>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<int?>("PositionId");

                    b.Property<int?>("RemainingDay");

                    b.Property<int?>("TeamId");

                    b.HasKey("EmployeeId");

                    b.HasIndex("ApartmentId");

                    b.HasIndex("PositionId");

                    b.HasIndex("TeamId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ProfileService.Model.Position", b =>
                {
                    b.Property<int>("PositionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PositionName");

                    b.HasKey("PositionId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("ProfileService.Model.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LeaderId");

                    b.Property<string>("TeamName");

                    b.HasKey("TeamId");

                    b.HasIndex("LeaderId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ProfileService.Model.Employee", b =>
                {
                    b.HasOne("ProfileService.Model.Apartment", "Apartment")
                        .WithMany()
                        .HasForeignKey("ApartmentId");

                    b.HasOne("ProfileService.Model.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.HasOne("ProfileService.Model.Team", "Team")
                        .WithMany("Employees")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("ProfileService.Model.Team", b =>
                {
                    b.HasOne("ProfileService.Model.Employee", "Leader")
                        .WithMany()
                        .HasForeignKey("LeaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
