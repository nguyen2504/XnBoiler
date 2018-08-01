using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xn.Controllers;
using Xn.Services;

namespace Xn.Web.Controllers
{
    public class GetController : XnControllerBase
    {
        private readonly INhapService _nhapService;

        public GetController(INhapService nhapService)
        {
            _nhapService = nhapService;
        }

        public IEnumerable<string> GetMaDh()
        {
          return  _nhapService.GetAll().Select(j => j.MaDonHang).Distinct();
        }

        [HttpPost]
        public IActionResult Dvts()
        {
            var dt= _nhapService.GetAll().Select(j => j.Dvt).Distinct();
            return Json(dt);
        }

       
    }
}