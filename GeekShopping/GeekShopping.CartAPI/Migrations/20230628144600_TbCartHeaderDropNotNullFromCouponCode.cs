using Microsoft.EntityFrameworkCore.Migrations;

namespace GeekShopping.CartAPI.Migrations
{
    public partial class TbCartHeaderDropNotNullFromCouponCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlDropNotNull = "ALTER TABLE cart_header ALTER COLUMN coupon_code DROP NOT NULL;";
            migrationBuilder.Sql(sqlDropNotNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
