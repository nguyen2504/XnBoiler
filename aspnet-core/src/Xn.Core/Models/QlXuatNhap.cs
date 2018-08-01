﻿using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using BaseEntity;

namespace Xn.Models
{
   public class QlXuatNhap:Entity
    {
        public string MaDonHang { get; set; }
        public double ThanhTien { get; set; }
        public double ThanhToan { get; set; }
        public DateTime NgayGhi { get; set; }
        public string Loai { get; set; }// xuat Nhap
        public int IdCty { get; set; }
        public bool IsActive { get; set; }
        public double Conlai { get; set; }
        public long CreateUserId { get; set; }
        public double Vat { get; set; }
    }
}
