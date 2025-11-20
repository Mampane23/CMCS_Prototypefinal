using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedClaimModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Claims",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LecturerName",
                table: "Claims",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ClaimMonth",
                table: "Claims",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Claims",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Claims",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LecturerEmail",
                table: "Claims",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleCode",
                table: "Claims",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedBy",
                table: "Claims",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedDate",
                table: "Claims",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "Claims",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_LecturerName",
                table: "Claims",
                column: "LecturerName");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_Status",
                table: "Claims",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_SubmissionDate",
                table: "Claims",
                column: "SubmissionDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Claims_LecturerName",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_Status",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_SubmissionDate",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ClaimMonth",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "LecturerEmail",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ModuleCode",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ReviewedDate",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "Claims");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "LecturerName",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
