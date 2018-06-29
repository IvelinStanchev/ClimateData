﻿// <auto-generated />
using MeteoApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace MeteoApp.Migrations.MeteoDataDB
{
    [DbContext(typeof(MeteoDataDBContext))]
    partial class MeteoDataDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MeteoApp.Data.Models.DayWeatherData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedOn");

                    b.Property<DateTime>("ChangedOn");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("Precipitation");

                    b.Property<int>("StationId");

                    b.Property<decimal>("Temperature");

                    b.Property<int>("ThunderCount");

                    b.Property<decimal>("Wind");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("DaysData");
                });

            modelBuilder.Entity("MeteoApp.Data.Models.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedOn");

                    b.Property<DateTime>("ChangedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("MeteoApp.Data.Models.StationAvailabilityPeriod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedOn");

                    b.Property<DateTime>("ChangedOn");

                    b.Property<DateTime>("From");

                    b.Property<int>("StationId");

                    b.Property<DateTime>("To");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("StationsAvailabilityPeriods");
                });

            modelBuilder.Entity("MeteoApp.Data.Models.StationWeight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedOn");

                    b.Property<DateTime>("ChangedOn");

                    b.Property<DateTime>("From");

                    b.Property<int>("StationId");

                    b.Property<DateTime>("To");

                    b.Property<decimal>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("StationsWeights");
                });

            modelBuilder.Entity("MeteoApp.Data.Models.DayWeatherData", b =>
                {
                    b.HasOne("MeteoApp.Data.Models.Station", "Station")
                        .WithMany("StationData")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MeteoApp.Data.Models.StationAvailabilityPeriod", b =>
                {
                    b.HasOne("MeteoApp.Data.Models.Station", "Station")
                        .WithMany("StationAvailabilityPeriods")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MeteoApp.Data.Models.StationWeight", b =>
                {
                    b.HasOne("MeteoApp.Data.Models.Station", "Station")
                        .WithMany("StationWeights")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}