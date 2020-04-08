using Microsoft.EntityFrameworkCore.Migrations;

namespace Profile.Api.DataAccess.Migrations
{
	public partial class UpdateCountryFieldToBeRequired : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Country",
				table: "Profiles",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Country",
				table: "Profiles",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldMaxLength: 100);
		}
	}
}
