using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using BaseEntity;

namespace Xn.Models
{
   public class NhapHang:Entity
    {
        public string TenNcc { get; set; }
        public int IdNcc { get; set; }
        public  string TenHang { get; set; }
        public string MaVt { get; set; }
        public int SoLuong { get; set; }
        public double DonGiaMua { get; set; }
        public string Dvt { get; set; }
        public DateTime NgayGhi { get; set; }
        public int IdCty { get; set; }
        public string MaDonHang { get; set; }
        public double Vat { get; set; }
        public bool IsActive { get; set; }
        public int IdNv { get; set; }
        public string TenNv { get; set; }
        public double ThanhToan { get; set; }
        public double ThanhTien { get; set; }
        public long CreateUserId { get; set; }
        public DateTime Ngay { get; set; }
        public string GhiChu { get; set; }
    }
}
