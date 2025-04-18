﻿// <auto-generated />
using System;
using KLTN_Team83.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KLTN_Team83.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KLTN_Team83.Models.Account", b =>
                {
                    b.Property<int>("id_Acc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id_Acc"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("passWord")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_Acc");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("KLTN_Team83.Models.Blog", b =>
                {
                    b.Property<int>("id_Blog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id_Blog"));

                    b.Property<string>("content")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("img")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ngayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("tilte")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("id_Blog");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("KLTN_Team83.Models.InfoUser", b =>
                {
                    b.Property<int>("id_User")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id_User"));

                    b.Property<int>("age")
                        .HasColumnType("int");

                    b.Property<string>("fullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("height")
                        .HasColumnType("float");

                    b.Property<int>("id_Account")
                        .HasColumnType("int");

                    b.Property<double>("weight")
                        .HasColumnType("float");

                    b.HasKey("id_User");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            id_User = 1,
                            age = 22,
                            fullName = "Nguyễn Lê Trung Khánh",
                            gender = "Male",
                            height = 176.0,
                            id_Account = 1,
                            weight = 84.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
