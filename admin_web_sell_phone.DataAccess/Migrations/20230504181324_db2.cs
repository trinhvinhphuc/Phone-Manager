using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace admin_web_sell_phone.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class db2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chitietphieunhap_ID_PhieuNhap",
                table: "Chitietphieunhap");

            migrationBuilder.DropIndex(
                name: "IX_Chitiethoadon_ID_HoaDon",
                table: "Chitiethoadon");

            migrationBuilder.AlterColumn<string>(
                name: "HinhAnh",
                table: "Sanpham",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IDchitietphieunhap",
                table: "Chitietphieunhap",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IDchitiethoadon",
                table: "Chitiethoadon",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chitietphieunhap",
                table: "Chitietphieunhap",
                columns: new[] { "ID_PhieuNhap", "ID_SanPham" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chitiethoadon",
                table: "Chitiethoadon",
                columns: new[] { "ID_HoaDon", "ID_SanPham" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Chitietphieunhap",
                table: "Chitietphieunhap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chitiethoadon",
                table: "Chitiethoadon");

            migrationBuilder.DropColumn(
                name: "IDchitietphieunhap",
                table: "Chitietphieunhap");

            migrationBuilder.DropColumn(
                name: "IDchitiethoadon",
                table: "Chitiethoadon");

            migrationBuilder.AlterColumn<string>(
                name: "HinhAnh",
                table: "Sanpham",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Chitietphieunhap_ID_PhieuNhap",
                table: "Chitietphieunhap",
                column: "ID_PhieuNhap");

            migrationBuilder.CreateIndex(
                name: "IX_Chitiethoadon_ID_HoaDon",
                table: "Chitiethoadon",
                column: "ID_HoaDon");
        }
    }
}
