using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBusket8.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addIsActivePropertyTOAnswerAndQuestionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Answers");
        }
    }
}
