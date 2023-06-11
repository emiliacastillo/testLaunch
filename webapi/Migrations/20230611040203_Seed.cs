using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "mission", columns: new[] { "ID", "MissionName" },
               values: new object[,] {
                    { 1,"m1" },
                    { 2,"m2" },
                    { 3,"m3" },
                    { 4,"m4" },
                    { 5,"m5" },
                    { 6,"m6" }
               });
            migrationBuilder.InsertData(table: "rocket", columns: new[] { "ID", "RateSucessRocket", "RocketName" },
               values: new object[,] {
                    { 1, 23, "rocket1" },
                    { 2, 35, "rocket2" },
                    { 3, 13, "rocket3" },
                    { 4, 3, "rocket4" }
               });
            migrationBuilder.InsertData(table: "launch", columns: new[] { "ID", "DateLunch", "MissionID", "RocketID", "FirstRocketlaunch" },
               values: new object[,] {
                    { 1,  "2023-06-21 05:15:43", 1 ,  1 ,  true },
                    { 2,  "2023-06-22 05:15:43", 2 ,  2 ,  true },
                    { 3,  "2023-06-21 05:15:43", 3 ,  3 ,  true },
                    { 4,  "2023-06-23 05:15:43", 4 ,  4 ,  true },
                    { 5,  "2023-06-21 05:15:43", 5 ,  1 ,  true },
                    { 6,  "2023-06-21 05:15:43", 6 ,  2 ,  true },
                    { 7,  "2023-06-24 05:15:43", 2 ,  3 ,  false },
                    { 8,  "2023-06-25 05:15:43", 1 ,  4 ,  false },
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "mission", keyColumns: new[] { "ID" }, keyValues: new object[,] { { 1 }, { 2 }, { 3 }, { 4 }, { 5 }, { 6 } });
            migrationBuilder.DeleteData(table: "rocket", keyColumns: new[] { "ID" }, keyValues: new object[,] { { 1 }, { 2 }, { 3 }, { 4 } });
            migrationBuilder.DeleteData(table: "launch", keyColumns: new[] { "ID" }, keyValues: new object[,] { { 1 }, { 2 }, { 3 }, { 4 }, { 5 }, { 6 }, { 7 }, { 8 } });

        }
    }
}
