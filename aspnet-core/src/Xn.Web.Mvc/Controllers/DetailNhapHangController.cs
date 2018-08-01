using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xn.Controllers;
using Xn.Services;

namespace Xn.Web.Controllers
{
    public class DetailNhapHangController : XnControllerBase
    {
        private readonly INhapService _nhapService;

        public DetailNhapHangController(INhapService nhapService)
        {
            _nhapService = nhapService;
        }
        public IActionResult Index()
        {
            return View();
        }
  
        public IActionResult Details(string madh)
        {
            var list = _nhapService.GetAll().Where(j => j.MaDonHang.Equals(madh)).ToList();
            return View("Index",list);
        }
    }
}