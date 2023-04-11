﻿// <auto-generated />
using Crawler.UrlRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Crawler.UrlRepository.Migrations
{
    [DbContext(typeof(CrawlerDatabaseContext))]
    [Migration("20230410193947_CreationOfTwoTables")]
    partial class CreationOfTwoTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Crawler.UrlDatabase.Entities.FoundUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InitialUrlId")
                        .HasColumnType("int");

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.Property<int>("ResponseTimeMs")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InitialUrlId");

                    b.ToTable("FoundUrls");
                });

            modelBuilder.Entity("Crawler.UrlDatabase.Entities.InitialUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BaseUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InitialUrls");
                });

            modelBuilder.Entity("Crawler.UrlDatabase.Entities.FoundUrl", b =>
                {
                    b.HasOne("Crawler.UrlDatabase.Entities.InitialUrl", "InitialUrl")
                        .WithMany("FoundUrls")
                        .HasForeignKey("InitialUrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InitialUrl");
                });

            modelBuilder.Entity("Crawler.UrlDatabase.Entities.InitialUrl", b =>
                {
                    b.Navigation("FoundUrls");
                });
#pragma warning restore 612, 618
        }
    }
}
