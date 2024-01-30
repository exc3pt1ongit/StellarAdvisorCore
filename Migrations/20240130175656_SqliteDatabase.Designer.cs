﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StellarAdvisorCore.Context;

#nullable disable

namespace StellarAdvisorCore.Migrations
{
    [DbContext(typeof(SqliteContext))]
    [Migration("20240130175656_SqliteDatabase")]
    partial class SqliteDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("StellarAdvisorCore.Models.MutedUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("MemberId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("MutedById")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("MutedExpiration")
                        .HasColumnType("TEXT");

                    b.Property<string>("MutedReason")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MutedUsers");
                });
#pragma warning restore 612, 618
        }
    }
}