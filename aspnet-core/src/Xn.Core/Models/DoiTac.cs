using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Domain.Entities;

namespace Xn.Models
{
  public  class DoiTac:Entity<long>
    {
        [Required]
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int IdCty { get; set; }
        public long CreateUserId { get; set; }
    }
}
