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
    [Migration("20210308044221_add-aeskey")]
    partial class addaeskey
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

                    b.ToTable("AesKey");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IV = new byte[] { 47, 229, 196, 4, 91, 184, 189, 79, 12, 248, 225, 192, 253, 28, 118, 211 },
                            Key = new byte[] { 231, 188, 153, 49, 147, 194, 81, 240, 141, 247, 144, 249, 104, 36, 170, 136, 125, 145, 160, 240, 247, 22, 207, 87, 227, 184, 170, 246, 48, 64, 118, 58 }
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