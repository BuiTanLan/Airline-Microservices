﻿// <auto-generated />
using System;
using Flight.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Flight.Data.Migrations
{
    [DbContext(typeof(FlightDbContext))]
    partial class FlightDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BuildingBlocks.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", (string)null);
                });

            modelBuilder.Entity("Flight.Aircrafts.Models.Aircraft", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<int>("ManufacturingYear")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Aircraft", "dbo");
                });

            modelBuilder.Entity("Flight.Airports.Models.Airport", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Airport", "dbo");
                });

            modelBuilder.Entity("Flight.Flights.Models.Flight", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<long>("AircraftId")
                        .HasColumnType("bigint");

                    b.Property<long>("ArriveAirportId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ArriveDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("DepartureAirportId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DurationMinutes")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("FlightDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.HasIndex("ArriveAirportId");

                    b.ToTable("Flight", "dbo");
                });

            modelBuilder.Entity("Flight.Seats.Models.Seat", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<int>("Class")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("FlightId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("SeatNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.ToTable("Seat", "dbo");
                });

            modelBuilder.Entity("Flight.Flights.Models.Flight", b =>
                {
                    b.HasOne("Flight.Aircrafts.Models.Aircraft", null)
                        .WithMany()
                        .HasForeignKey("AircraftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flight.Airports.Models.Airport", null)
                        .WithMany()
                        .HasForeignKey("ArriveAirportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Flight.Seats.Models.Seat", b =>
                {
                    b.HasOne("Flight.Flights.Models.Flight", null)
                        .WithMany()
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
