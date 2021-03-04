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
    [Migration("20210304031249_RenameEntity_FilePath_To_EncryptedFile")]
    partial class RenameEntity_FilePath_To_EncryptedFile
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
                        .HasColumnType("TEXT");

                    b.Property<string>("FullPath")
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
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
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
