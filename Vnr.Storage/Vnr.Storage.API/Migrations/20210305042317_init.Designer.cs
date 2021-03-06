﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vnr.Storage.API.Infrastructure.Data;

namespace Vnr.Storage.API.Migrations
{
    [DbContext(typeof(StorageContext))]
    [Migration("20210305042317_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("Vnr.Storage.API.Infrastructure.Data.Entities.EncryptedFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FullPath")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EncryptedFiles");
                });

            modelBuilder.Entity("Vnr.Storage.API.Infrastructure.Data.Entities.RijndaelKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IV")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RijndaelKeys");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IV = "QtgaRmKYbpvV4cPp3DtU/g==",
                            Key = "fjc0nLMh5acJhSz5XjN4X6zExZUShzYN11Twf6VSwFE="
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
