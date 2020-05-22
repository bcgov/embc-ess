using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class AddBigLakeCommunity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
              IF NOT EXISTS (SELECT 1 FROM Communities WHERE [Name] = 'Big Lake')
              BEGIN
                INSERT INTO Communities (Id, Name, Active, RegionName, CreatedByUserId, CreatedDateTime)
	            VALUES (NEWID(), 'Big Lake', 1, 'Central' , 'System', GETUTCDATE())
              END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
              IF EXISTS (SELECT 1 FROM Communities WHERE [Name] = 'Big Lake')
              BEGIN
	            DELETE Communities
	            WHERE [Name] = 'Big Lake'
              END
            ");
        }
    }
}
