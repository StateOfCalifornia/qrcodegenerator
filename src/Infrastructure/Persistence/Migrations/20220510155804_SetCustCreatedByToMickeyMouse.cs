using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class SetCustCreatedByToMickeyMouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"UPDATE Customers SET CreatedBy = 'Mickey.Mouse'";
            migrationBuilder.Sql(sql);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
