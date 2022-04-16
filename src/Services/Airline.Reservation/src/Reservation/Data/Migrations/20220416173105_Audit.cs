using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservation.Data.Migrations
{
    public partial class Audit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                schema: "dbo",
                table: "Reservation",
                newName: "LastModifiedBy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                schema: "dbo",
                table: "Reservation",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Reservation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "dbo",
                table: "Reservation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                schema: "dbo",
                table: "Reservation",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "dbo",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "dbo",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "dbo",
                table: "Reservation",
                newName: "ModifiedBy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                schema: "dbo",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
