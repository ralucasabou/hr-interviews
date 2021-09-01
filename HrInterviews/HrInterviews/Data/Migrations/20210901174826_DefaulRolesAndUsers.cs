using HrInterviews.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrInterviews.Data.Migrations
{
    public partial class DefaulRolesAndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddRole("admin");
            migrationBuilder.AddRole("operator");

            migrationBuilder.AddUserWithRoles(
                "ralucasabou@yahoo.com",
                "Raluca123.",
                new[] { "admin" });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
