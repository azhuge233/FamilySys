﻿// <auto-generated />
using System;
using FamilySys.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FamilySys.Migrations
{
    [DbContext(typeof(FamilySysDbContext))]
    partial class FamilySysDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("FamilySys.Models.DbModels.Bark", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<bool>("is_https");

                    b.HasKey("Id");

                    b.ToTable("Barks");
                });

            modelBuilder.Entity("FamilySys.Models.DbModels.Dream", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5);

                    b.Property<int>("Agree");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<int>("Veto");

                    b.HasKey("ID");

                    b.ToTable("Dreams");
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

            modelBuilder.Entity("FamilySys.Models.DbModels.MonthlyRank", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10);

                    b.Property<DateTime>("Date");

                    b.Property<int>("Rank");

                    b.Property<int>("Score");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.ToTable("MonthlyRanks");
                });

            modelBuilder.Entity("FamilySys.Models.DbModels.Rate", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5);

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<string>("FromID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("HouseworkID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<int>("Star");

                    b.Property<string>("ToID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("FamilySys.Models.DbModels.ScoreRecord", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5);

                    b.Property<DateTime>("Date");

                    b.Property<string>("HouseworkID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("RateID")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<int>("Score");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.ToTable("ScoreRecords");
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

            modelBuilder.Entity("FamilySys.Models.DbModels.UserDreamVote", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5);

                    b.Property<string>("DreamID")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<bool>("IsAgree");

                    b.Property<bool>("IsVeto");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.ToTable("UserDreamVotes");
                });
#pragma warning restore 612, 618
        }
    }
}
