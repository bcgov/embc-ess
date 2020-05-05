using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class AddStrathconaRegionalDistrictToCommunities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
              IF NOT EXISTS (SELECT 1 FROM Communities WHERE [Name] = 'Strathcona Regional District')
              BEGIN
                INSERT INTO Communities (Id, Name, Active, RegionName, CreatedByUserId, CreatedDateTime)
	            VALUES (NEWID(), 'Strathcona Regional District', 1, 'Vancouver Island' , 'System', GETUTCDATE())
              END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
              IF EXISTS (SELECT 1 FROM Communities WHERE [Name] = 'Strathcona Regional District')
              BEGIN
	            DELETE Communities
	            WHERE [Name] = 'Strathcona Regional District'
              END
            ");
        }
    }
}
