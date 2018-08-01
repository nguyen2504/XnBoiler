using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Xn.Migrations
{
    public partial class InitialNhapHangs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NhapHangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateUserId = table.Column<long>(nullable: false),
                    DonGiaMua = table.Column<double>(nullable: false),
                    Dvt = table.Column<string>(nullable: true),
                    IdCty = table.Column<int>(nullable: false),
                    IdNcc = table.Column<int>(nullable: false),
                    IdNv = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    MaDonHang = table.Column<string>(nullable: true),
                    MaVt = table.Column<string>(nullable: true),
                    NgayGhi = table.Column<DateTime>(nullable: false),
                    SoLuong = table.Column<int>(nullable: false),
                    TenHang = table.Column<string>(nullable: true),
                    TenNcc = table.Column<string>(nullable: true),
                    TenNv = table.Column<string>(nullable: true),
                    ThanhToan = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhapHangs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NhapHangs");
        }
    }
}
