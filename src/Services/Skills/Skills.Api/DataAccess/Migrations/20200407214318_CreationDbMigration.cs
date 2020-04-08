using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skills.Api.DataAccess.Migrations
{
	public partial class CreationDbMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Professions",
				columns: table => new
				{
					Name = table.Column<string>(maxLength: 50, nullable: false),
					Description = table.Column<string>(maxLength: 250, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Professions", x => x.Name);
				});

			migrationBuilder.CreateTable(
				name: "ProfessionSkillAvailable",
				columns: table => new
				{
					ProfessionName = table.Column<string>(maxLength: 50, nullable: false),
					SkillLevelName = table.Column<string>(maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ProfessionSkillAvailable", x => new { x.ProfessionName, x.SkillLevelName });
				});

			migrationBuilder.CreateTable(
				name: "SkillLevels",
				columns: table => new
				{
					Name = table.Column<string>(maxLength: 50, nullable: false),
					Description = table.Column<string>(maxLength: 250, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SkillLevels", x => x.Name);
				});

			migrationBuilder.CreateTable(
				name: "UserSkills",
				columns: table => new
				{
					UserId = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserSkills", x => x.UserId);
				});

			migrationBuilder.CreateTable(
				name: "Skills",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					ProfessionName = table.Column<string>(nullable: true),
					SkillLevelName = table.Column<string>(nullable: true),
					UserSkillsUserId = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Skills", x => x.Id);
					table.ForeignKey(
						name: "FK_Skills_Professions_ProfessionName",
						column: x => x.ProfessionName,
						principalTable: "Professions",
						principalColumn: "Name",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Skills_SkillLevels_SkillLevelName",
						column: x => x.SkillLevelName,
						principalTable: "SkillLevels",
						principalColumn: "Name",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Skills_UserSkills_UserSkillsUserId",
						column: x => x.UserSkillsUserId,
						principalTable: "UserSkills",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Skills_ProfessionName",
				table: "Skills",
				column: "ProfessionName");

			migrationBuilder.CreateIndex(
				name: "IX_Skills_SkillLevelName",
				table: "Skills",
				column: "SkillLevelName");

			migrationBuilder.CreateIndex(
				name: "IX_Skills_UserSkillsUserId",
				table: "Skills",
				column: "UserSkillsUserId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ProfessionSkillAvailable");

			migrationBuilder.DropTable(
				name: "Skills");

			migrationBuilder.DropTable(
				name: "Professions");

			migrationBuilder.DropTable(
				name: "SkillLevels");

			migrationBuilder.DropTable(
				name: "UserSkills");
		}
	}
}
