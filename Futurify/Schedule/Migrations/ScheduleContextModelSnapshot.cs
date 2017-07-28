using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Schedule.Models;

namespace Schedule.Migrations
{
    [DbContext(typeof(ScheduleContext))]
    partial class ScheduleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Schedule.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("End");

                    b.Property<bool>("IsAllDay");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int?>("OwnerId");

                    b.Property<DateTime?>("Start");

                    b.Property<string>("Title");

                    b.HasKey("EventId");

                    b.ToTable("Vacations");
                });
        }
    }
}
