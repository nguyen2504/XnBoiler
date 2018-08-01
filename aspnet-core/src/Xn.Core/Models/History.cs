using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Xn.Models
{
  public  class History:Entity<long>
    {
      public string Code { get; set; }
        public string KeyUser { get; set; }
        public string NgayGhi { get; set; }
        public string GhiChu { get; set; }
        public string NoiDung { get; set; }
        public int IdCty { get; set; }
        public long CreateUserId { get; set; }
    }
}
