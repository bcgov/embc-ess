using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
	public partial class RemoveMisspelledCommunities : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				IF EXISTS (SELECT 1 FROM Communities WHERE [Name] = 'Xat''s?ll (Soda Creek First Nations)')
				BEGIN
					DELETE Communities
					WHERE [Name] = 'Xat''s?ll (Soda Creek First Nations)'
				END

				IF EXISTS (SELECT 1 FROM Communities WHERE [Name] = 'Yunes?it''in')
				BEGIN
					DELETE Communities
					WHERE [Name] = 'Yunes?it''in'
				END

				IF EXISTS (SELECT 1 FROM Communities WHERE [Name] = 'Ts?i Deldel')
				BEGIN
					DELETE Communities
					WHERE [Name] = 'Ts?i Deldel'
				END
			");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
