using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Notification.Models;

namespace Notification.Migrations
{
    [DbContext(typeof(MessageContext))]
    partial class MessageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Notification.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime?>("CreateAt");

                    b.Property<DateTime?>("ReadAt");

                    b.Property<int?>("ReceiverId");

                    b.Property<int?>("SenderId");

                    b.Property<string>("Subject");

                    b.Property<int?>("Type");

                    b.HasKey("Id");

                    b.ToTable("Message");
                });
        }
    }
}
