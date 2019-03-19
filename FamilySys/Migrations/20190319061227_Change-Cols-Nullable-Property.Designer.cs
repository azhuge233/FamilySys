﻿// <auto-generated />
using System;
using FamilySys.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FamilySys.Migrations
{
    [DbContext(typeof(FamilySysDbContext))]
    [Migration("20190319061227_Change-Cols-Nullable-Property")]
    partial class ChangeColsNullableProperty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FamilySys.Models.DbModels.Announcement", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("ModifyDate");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("FamilySys.Models.DbModels.Housework", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("Date");

                    b.Property<string>("FromID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<bool>("IsDone");

                    b.Property<DateTime>("ModifyDate");

                    b.Property<int>("Score");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("ToID")
                        .HasMaxLength(10);

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.ToTable("Houseworks");
                });

            modelBuilder.Entity("FamilySys.Models.DbModels.User", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10);

                    b.Property<int>("IsAdmin");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Phone")
                        .HasMaxLength(15);

                    b.Property<int>("Score");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
