using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xn.Models;

namespace Xn.Web.Models
{
    public class NhapHangQlNh
    {
        public List<NhapHangEntity> Nhaps { get; set; }
        public QlXuatNhap QlXuatNhap { get; set; }
    }
}
