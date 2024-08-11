﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleNewTab.Api.Data;

#nullable disable

namespace SimpleNewTab.Api.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240810193313_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("SimpleNewTab.Api.Common.ImageMetadata", b =>
                {
                    b.Property<long>("Expiration")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Copyright")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CopyrightUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("QuizUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Expiration");

                    b.ToTable("ImageMetadata");
                });
#pragma warning restore 612, 618
        }
    }
}
