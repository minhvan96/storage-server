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
    [Migration("20210308045050_addaes")]
    partial class addaes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("Vnr.Storage.API.Infrastructure.Data.Entities.AesKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("IV")
                        .HasMaxLength(40)
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("Key")
                        .HasMaxLength(40)
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.ToTable("AesKeys");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IV = new byte[] { 60, 194, 198, 149, 105, 243, 198, 134, 105, 137, 179, 13, 229, 165, 139, 52 },
                            Key = new byte[] { 28, 117, 42, 194, 1, 117, 71, 68, 139, 169, 0, 154, 31, 28, 204, 29, 82, 77, 122, 1, 117, 73, 234, 115, 68, 255, 108, 46, 85, 111, 150, 165 }
                        });
                });

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