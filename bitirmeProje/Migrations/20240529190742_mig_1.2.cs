using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bitirmeProje.Migrations
{
    /// <inheritdoc />
    public partial class mig_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionType_question_type_id",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_District_district_id",
                table: "Users");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "QuestionType");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Users_district_id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Question_question_type_id",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "age",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "district_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "question_type_id",
                table: "Question");

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "Group",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "district_id",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "question_type_id",
                table: "Question",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "Group",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    question_type_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    city_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.id);
                    table.ForeignKey(
                        name: "FK_City_Country_country_id",
                        column: x => x.country_id,
                        principalTable: "Country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    city_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    district_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.id);
                    table.ForeignKey(
                        name: "FK_District_City_city_id",
                        column: x => x.city_id,
                        principalTable: "City",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_district_id",
                table: "Users",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "IX_Question_question_type_id",
                table: "Question",
                column: "question_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_City_country_id",
                table: "City",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_District_city_id",
                table: "District",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionType_question_type_id",
                table: "Question",
                column: "question_type_id",
                principalTable: "QuestionType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_District_district_id",
                table: "Users",
                column: "district_id",
                principalTable: "District",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
