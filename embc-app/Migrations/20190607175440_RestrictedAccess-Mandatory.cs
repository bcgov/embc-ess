using Microsoft.EntityFrameworkCore.Migrations;

namespace Gov.Jag.Embc.Public.Migrations
{
    public partial class RestrictedAccessMandatory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE EvacueeRegistrations SET RestrictedAccess=0 WHERE RestrictedAccess IS NULL");
            migrationBuilder.AlterColumn<bool>(
                name: "RestrictedAccess",
                table: "EvacueeRegistrations",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "RestrictedAccess",
                table: "EvacueeRegistrations",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
