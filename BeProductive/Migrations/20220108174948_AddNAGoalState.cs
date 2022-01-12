using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeProductive.Migrations
{
    public partial class AddNAGoalState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:goal_state", "unknown,Success,Failure,Emergency,NotApplicable")
                .OldAnnotation("Npgsql:Enum:goal_state", "unknown,Success,Failure,Emergency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:goal_state", "unknown,Success,Failure,Emergency")
                .OldAnnotation("Npgsql:Enum:goal_state", "unknown,Success,Failure,Emergency,NotApplicable");
        }
    }
}
