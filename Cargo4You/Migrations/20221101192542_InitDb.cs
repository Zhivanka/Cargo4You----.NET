using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cargo4You.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargoPartners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DimensionPrice = table.Column<double>(type: "float", nullable: true),
                    WeightPrice = table.Column<double>(type: "float", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    ValidationKg1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidationKg2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationCm3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalculationKgString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalculationCm3String = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoPartners", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CargoPartners",
                columns: new[] { "Id", "CalculationCm3String", "CalculationKgString", "DimensionPrice", "Name", "Price", "ValidationCm3", "ValidationKg1", "ValidationKg2", "WeightPrice" },
                values: new object[] { 1, "<=1000price10;>1000&&<=2000price20", "<=2price15;>2&&<=15price18;>15&&<=20price35", 0.0, "Cargo 4 You", 0.0, "<=2000", "<=20", "", 0.0 });

            migrationBuilder.InsertData(
                table: "CargoPartners",
                columns: new[] { "Id", "CalculationCm3String", "CalculationKgString", "DimensionPrice", "Name", "Price", "ValidationCm3", "ValidationKg1", "ValidationKg2", "WeightPrice" },
                values: new object[] { 2, "<=1000price11,99;>1000&&<=1700price21,99", ">10&&<=15price16,5;>15&&<=25price36,5;>25price40+0,417", 0.0, "Ship Faster", 0.0, "<=1700", ">10", "<=30", 0.0 });

            migrationBuilder.InsertData(
                table: "CargoPartners",
                columns: new[] { "Id", "CalculationCm3String", "CalculationKgString", "DimensionPrice", "Name", "Price", "ValidationCm3", "ValidationKg1", "ValidationKg2", "WeightPrice" },
                values: new object[] { 3, "<=1000price9,5;>1000&&<=2000price19,5;>2000&&<=5000price48,5;>5000price147,5", ">=10&&<=20price16,99;>20&&<=30price33,99;>30price43,99+0,41", 0.0, "Malta Ship", 0.0, ">=500", ">=10", "", 0.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargoPartners");
        }
    }
}
