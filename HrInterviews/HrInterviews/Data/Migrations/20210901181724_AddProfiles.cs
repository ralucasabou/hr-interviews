using Microsoft.EntityFrameworkCore.Migrations;

namespace HrInterviews.Data.Migrations
{
    public partial class AddProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Studies = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PastExperiene = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DrivingLicense = table.Column<bool>(type: "bit", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
