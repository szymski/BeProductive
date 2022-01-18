using System;
using BeProductive.Modules.Rewards.Domain;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BeProductive.Migrations
{
    public partial class AddRewardAndPointEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:goal_state", "unknown,Success,Failure,Emergency,NotApplicable")
                .Annotation("Npgsql:Enum:point_claim_source_type", "seed,goal_day_state")
                .OldAnnotation("Npgsql:Enum:goal_state", "unknown,Success,Failure,Emergency,NotApplicable");

            migrationBuilder.CreateTable(
                name: "PointClaimEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PointsClaimed = table.Column<int>(type: "integer", nullable: false),
                    TotalBalance = table.Column<int>(type: "integer", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PointClaimSourceType = table.Column<PointClaimSourceType>(type: "point_claim_source_type", nullable: false),
                    SourceGoalId = table.Column<int>(type: "integer", nullable: true),
                    SourceDay = table.Column<DateOnly>(type: "date", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointClaimEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointClaimEvents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    IsSingleUse = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rewards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointClaimEvents_UserId",
                table: "PointClaimEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_UserId",
                table: "Rewards",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointClaimEvents");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:goal_state", "unknown,Success,Failure,Emergency,NotApplicable")
                .OldAnnotation("Npgsql:Enum:goal_state", "unknown,Success,Failure,Emergency,NotApplicable")
                .OldAnnotation("Npgsql:Enum:point_claim_source_type", "seed,goal_day_state");
        }
    }
}
