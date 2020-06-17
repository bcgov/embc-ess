using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class UpdateLakeBabineToBabineLake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Communities
                SET [Name] = 'Babine Lake'
                WHERE [Name] = 'Lake Babine'
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Communities
                SET [Name] = 'Lake Babine'
                WHERE [Name] = 'Babine Lake'
            ");
        }
    }
}
