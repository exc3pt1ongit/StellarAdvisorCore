﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StellarAdvisorCore.Data.Context;

#nullable disable

namespace StellarAdvisorCore.Migrations
{
    [DbContext(typeof(SqliteContext))]
    [Migration("20240201122346_InitializeSqliteDB")]
    partial class InitializeSqliteDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("StellarAdvisorCore.Data.Models.Entities.Characters.Character", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("DiscordUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Faction")
                        .HasColumnType("TEXT");

                    b.Property<string>("FactionRole")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Settlement")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SettlementBaseId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SettlementBaseId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("StellarAdvisorCore.Data.Models.Entities.Settlements.SettlementBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Settlements");
                });

            modelBuilder.Entity("StellarAdvisorCore.Data.Models.MutedUser", b =>
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

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MutedUsers");
                });

            modelBuilder.Entity("StellarAdvisorCore.Data.Models.Entities.Characters.Character", b =>
                {
                    b.HasOne("StellarAdvisorCore.Data.Models.Entities.Settlements.SettlementBase", null)
                        .WithMany("Residents")
                        .HasForeignKey("SettlementBaseId");
                });

            modelBuilder.Entity("StellarAdvisorCore.Data.Models.Entities.Settlements.SettlementBase", b =>
                {
                    b.Navigation("Residents");
                });
#pragma warning restore 612, 618
        }
    }
}
