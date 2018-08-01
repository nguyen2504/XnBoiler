using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Domain.Entities;

namespace Xn.Models
{
   public class ChiphiDt:Entity<long>
    {
        [Required]
        public string TenChiPhi { get; set; }
        public string GhiChu { get; set; }
        public double SoTien { get; set; }
        public string TenXe { get; set; }
        public string TenTx { get; set; }
        public string TenDt { get; set; }
        public int IdDoitac { get; set; }
        [Required]
        public DateTime NgayGhi { get; set; }
        public string Code { get; set; }
        public string TenNvGhi { get; set; }
        public string KeyUser { get; set; }
        public string Loai { get; set; }
        public double LoiNhuan { get; set; }
        public int IdCty { get; set; }
        public long CreateUserId { get; set; }

    }
}
