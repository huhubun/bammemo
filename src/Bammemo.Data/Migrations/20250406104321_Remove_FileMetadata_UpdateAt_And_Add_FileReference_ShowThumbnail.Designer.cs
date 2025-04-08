﻿// <auto-generated />
using Bammemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bammemo.Data.Migrations
{
    [DbContext(typeof(BammemoDbContext))]
    [Migration("20250406104321_Remove_FileMetadata_UpdateAt_And_Add_FileReference_ShowThumbnail")]
    partial class Remove_FileMetadata_UpdateAt_And_Add_FileReference_ShowThumbnail
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("Bammemo.Data.Entities.FileMetadata", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FileType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HashAlgorithm")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HashValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StorageFileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("StorageType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Path", "FileName");

                    b.ToTable("FileMetadata");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.FileReference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MetadataId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("ShowThumbnail")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SourceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SourceType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MetadataId");

                    b.HasIndex("SourceType", "SourceId");

                    b.ToTable("FileReferences");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.RedirectRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HttpStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Source")
                        .IsUnique();

                    b.ToTable("RedirectRules");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("UpdateAt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Key");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.SiteLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("UpdateAt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SiteLinks");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.Slip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Excerpt")
                        .HasColumnType("TEXT");

                    b.Property<string>("FriendlyLinkName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<long?>("UpdateAt")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.ToTable("Slips");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.SlipTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SlipId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SlipId");

                    b.HasIndex("Tag", "SlipId");

                    b.ToTable("SlipTags");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.FileReference", b =>
                {
                    b.HasOne("Bammemo.Data.Entities.FileMetadata", "Metadata")
                        .WithMany("References")
                        .HasForeignKey("MetadataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Metadata");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.SlipTag", b =>
                {
                    b.HasOne("Bammemo.Data.Entities.Slip", "Slip")
                        .WithMany("Tags")
                        .HasForeignKey("SlipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Slip");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.FileMetadata", b =>
                {
                    b.Navigation("References");
                });

            modelBuilder.Entity("Bammemo.Data.Entities.Slip", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
