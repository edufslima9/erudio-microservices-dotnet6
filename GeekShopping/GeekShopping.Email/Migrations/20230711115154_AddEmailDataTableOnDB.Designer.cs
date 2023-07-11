﻿// <auto-generated />
using System;
using GeekShopping.Email.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeekShopping.Email.Migrations
{
    [DbContext(typeof(PostgreSQLContext))]
    [Migration("20230711115154_AddEmailDataTableOnDB")]
    partial class AddEmailDataTableOnDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GeekShopping.Email.Models.EmailLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Log")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("log");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("sent_date");

                    b.HasKey("Id");

                    b.ToTable("email_logs");
                });
#pragma warning restore 612, 618
        }
    }
}
