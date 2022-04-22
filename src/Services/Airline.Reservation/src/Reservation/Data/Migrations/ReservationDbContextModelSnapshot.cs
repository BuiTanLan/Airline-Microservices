﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reservation.Data;

#nullable disable

namespace Reservation.Data.Migrations
{
    [DbContext(typeof(ReservationDbContext))]
    partial class ReservationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BuildingBlocks.InternalProcessor.InternalMessage", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CommandType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("EventId");

                    b.ToTable("InternalMessages", (string)null);
                });

            modelBuilder.Entity("BuildingBlocks.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("EventId")
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

                    b.HasKey("EventId");

                    b.ToTable("OutboxMessages", (string)null);
                });

            modelBuilder.Entity("Reservation.Reservations.Models.Reservation", b =>
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

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Reservation", "dbo");
                });

            modelBuilder.Entity("Reservation.Reservations.Models.Reservation", b =>
                {
                    b.OwnsOne("Reservation.Reservations.Models.ValueObjects.PassengerInfo", "PassengerInfo", b1 =>
                        {
                            b1.Property<long>("ReservationId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ReservationId");

                            b1.ToTable("Reservation", "dbo");

                            b1.WithOwner()
                                .HasForeignKey("ReservationId");
                        });

                    b.OwnsOne("Reservation.Reservations.Models.ValueObjects.Trip", "Trip", b1 =>
                        {
                            b1.Property<long>("ReservationId")
                                .HasColumnType("bigint");

                            b1.Property<long>("AircraftId")
                                .HasColumnType("bigint");

                            b1.Property<long>("ArriveAirportId")
                                .HasColumnType("bigint");

                            b1.Property<long>("DepartureAirportId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Description")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("FlightDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("FlightNumber")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<decimal>("Price")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<string>("SeatNumber")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ReservationId");

                            b1.ToTable("Reservation", "dbo");

                            b1.WithOwner()
                                .HasForeignKey("ReservationId");
                        });

                    b.Navigation("PassengerInfo");

                    b.Navigation("Trip");
                });
#pragma warning restore 612, 618
        }
    }
}
