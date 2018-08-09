using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Castle.Components.DictionaryAdapter;
using Functions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUglify.Helpers;
using Xn.Authorization.Users;
using Xn.Controllers;
using Xn.Models;
using Xn.Services;
using Xn.Web.Models;
using Convert = System.Convert;

namespace Xn.Web.Controllers
{
    public class NhapController : XnControllerBase
    {
        private readonly INhapService _nhap;
        private readonly IQlNhapXuatService _qlNx;
        //private readonly INccService _nccService;
        private readonly UserManager _user;
        private readonly INccService _ncc;
        private readonly IGetIdService _getIdService;
        private IHostingEnvironment _hostingEnvironment;
        List<NhapHang> _list = new List<NhapHang>();
        private string _all ="All";
        public NhapController(INhapService nhap, IQlNhapXuatService qlNx, UserManager user, INccService ncc, IGetIdService getIdService
            ,IHostingEnvironment hostingEnvironment)
        {
            _nhap = nhap;
            _qlNx = qlNx;
            _user = user;
            _ncc = ncc;
            _getIdService = getIdService;
            _hostingEnvironment = hostingEnvironment;
        }
        public int IdCty()
        {
            return int.Parse(_user.AbpSession.UserId.ToString());
        }
        public IActionResult Index()
        {
            var ncc = _ncc.GetAll().ToList().Select(j => j.TenNcc).Distinct().ToList();
            var tenhangs = _nhap.GetAll().Select(j => j.TenHang).Distinct().ToList();
            var madh = _nhap.GetAll().Select(j => j.MaDonHang).Distinct().ToList();
            var t = new ReportsIndex() {TenNccs = ncc,TenHangs = tenhangs,DonHangs = madh};
            //ViewBag.nccs = ncc;
            return View(t);
        }

      
        public IActionResult Creates()
        {
           return View();
        }

        public IActionResult Xoaedit(int id)
        {
            //var kt = _nhap.GetAll(IdCty()).FirstOrDefault(j => j.Id.Equals(id));
            //if (kt != null)
            //{
            //    kt.IsActive = false;
            //    _nhap.UpdateId(kt);
            //    var mdh = kt.MaDonHang;
            //    var edit = _qlNx.GetAllListGetIdcty_madh(IdCty(), mdh).FirstOrDefault();
            //    if (edit != null)
            //    {
            //        //var kt1 = _nhap.GetAll(IdCty()).FirstOrDefault(j => j.Id.Equals(id));
            //        var tt = edit.ThanhTien - kt.DonGiaMua * kt.SoLuong;
            //        edit.ThanhTien = tt;
            //        edit.Conlai = tt - edit.ThanhToan;
            //        _qlNx.Update(edit);
            //    }
            //}
            return Json("d");
        }
        public IActionResult GetMaDh()
        {
            var id = _user.AbpSession.UserId;
            var name = _user.Users.FirstOrDefault(j => j.Id.Equals(id));
            var date = DateTime.Today;
            var madh = "";
            var moth = "";
            if (date.Month < 10)
            {
                moth = "0" + date.Month;}
            else
            {
                moth =  date.Month.ToString();
            }

            var t = _nhap.GetAll().ToList()
                .Where(j => j.NgayGhi.Month.Equals(date.Month) && j.NgayGhi.Year.Equals(date.Year)).ToList();
            var num = 0;
            if (t.Count.Equals(0))
            {
                num = 1;}
            else
            {
                try
                {
                    num = Int32.Parse(t.LastOrDefault().MaDonHang[1].ToString());
                }
                catch (Exception e)
                {
                    num = Int32.Parse(t.LastOrDefault().MaDonHang[0].ToString());
                }
            }
            var stt = num+1;
            if (stt < 10)
            {
                madh += "0" + stt;
            }
            else
            {
                madh += (stt);
            }
            ;
            //if (name != null)
            //{
            //    madh += name.Name[0].ToString().ToUpper();
            //}
          
            return  Json(madh+"PN" + date.Year.ToString()[2] + date.Year.ToString()[3] + moth );
        }

        [HttpPost]
        public IActionResult Dvts()
        {
            var dt = _nhap.GetAll().Select(j => j.Dvt).Distinct();
            return Json(dt);
        }
        [HttpPost]
        public IActionResult CreateOrEdit([FromBody] List<NhapHangEntity> entity)
        {
            
            var id = _user.AbpSession.UserId;
            var name = _user.Users.FirstOrDefault(j => j.Id.Equals(id))?.FullName;
            var th = new List<NhapHang>();
            foreach (var w in entity)
            {
                if (w.TenHang != null)
                {   
                    w.CreateUserId = _getIdService.CreateIdUser();
                    w.IdCty = IdCty();
                    w.IdNv = (int)id;
                    w.TenNv = name;
                    w.NgayGhi = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                    w.TenNcc = entity[entity.Count - 1].TenNcc;
                    w.IdNcc = _ncc.GetAll().FirstOrDefault(j => j.TenNcc.Equals(entity[entity.Count - 1].TenNcc)).IdCty;
                    w.ThanhToan = 0;
                    w.IsActive = true;
                    w.Vat = w.Vat.Split('%')[0];
                    var ouput = w.MapTo<NhapHang>();
                    ouput.Vat = double.Parse(w.Vat.Split('%')[0])/100;
                    ouput.Ngay = DateTime.Now;
                   th.Add(ouput);
                }
                else
                {
                    
                }
               
            }
            _nhap.CreateOrUpdates(th);
            var qlnx = new QlXuatNhap()
            {
                MaDonHang = entity[0].MaDonHang,
                IsActive = true,
                IdCty = IdCty(),
                Loai = "Nhap",
                ThanhTien = entity[entity.Count - 1].SoLuong,
                Vat = double.Parse(entity[0].Vat.Split('%')[0]) / 100,
                ThanhToan = entity[entity.Count - 1].DonGiaMua,
                Conlai = entity[entity.Count - 1].SoLuong - entity[entity.Count - 1].DonGiaMua,
                NgayGhi = DateTime.Now,
                Id = (int) entity[entity.Count - 1].Id,
                CreateUserId = _getIdService.CreateIdUser()
            };
            _qlNx.CreateOrUpdate(qlnx);
            return Content("thanh cong");
        }

        public void Edit([FromBody] List<NhapHangEntity> entity)
        {
            var th = new List<NhapHang>();
            for (int i = 0; i < entity.Count-1; i++)
            {
                var t = entity[i].MapTo<NhapHang>();
                //t.MaDonHang = entity[i].MaDonHang;
                t.IdNv = IdCty();
                t.TenNcc = entity[entity.Count - 1].TenNcc;
                t.TenNv = _user.Users.SingleOrDefault(j =>j.Id.Equals(_user.AbpSession.UserId)).FullName;
                t.NgayGhi = Convert.ToDateTime(entity[i].NgayGhi);
                th.Add(t);
            }
         
            //_nhap.CreatesOrUpdates(th);
            var t1 = entity[entity.Count - 1];
            if (t1 != null)
            {
                var h = _qlNx.GetAll(IdCty()).FirstOrDefault();
                if (h != null)
                {
                    h.IsActive = true;
                   
                    h.ThanhTien = t1.SoLuong;
                    h.ThanhToan = t1.DonGiaMua;
                    h.Conlai = (t1.SoLuong - t1.DonGiaMua);
                    _qlNx.Update(h);
                }
            }
        }
        //---------------------------------------------//
        public IActionResult GetSearching([FromBody] NhapHangRequet requet)
        {
            if (requet.MaDh == null) requet.MaDh = _all;
            if (requet.Name == null) requet.Name = _all;

            
            var dt = _nhap.GetAll().Where(j => (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
            //1 -- All,All
            _list = dt;
            //2 All, #
            if (requet.MaDh == _all && requet.Name != _all)
            {
                _list = dt.Where(j =>j.TenHang.ToLower().Equals(requet.Name.ToLower()) || j.MaDonHang.ToLower().Equals(requet.Name.ToLower()) ).ToList();
            }
            //3 #,All
            if (requet.MaDh != _all && requet.Name == _all)
            {
                _list = dt.Where(j => j.TenNcc.ToLower().Equals(requet.MaDh.ToLower())).ToList();
            }
            // 4 #,#
            if (requet.MaDh != _all && requet.Name != _all)
            {
                _list = dt.Where(j => j.TenNcc.ToLower().Equals(requet.MaDh.ToLower()) &&(j.TenHang.ToLower().Equals(requet.Name.ToLower()) || j.MaDonHang.ToLower().Equals(requet.Name.ToLower()))).ToList();
            }
            //if (requet.MaDh == "All" || requet.Name == "All")
            //{
            //    dt = _nhap.GetAll().Where(j =>
            //                                   (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList();
            //    _list = dt;
            //}
            //if (requet.Name.Equals("0") && !requet.MaDh.Equals("All"))
            //{
            //    var dt1 = _nhap.GetAll().Where(j => (j.TenNcc.Equals(requet.MaDh))
            //                                       && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList();
            //    _list = dt1;
            //    return Json(dt1);
            //}

            //if (requet.Name.Equals("0") && requet.MaDh.Equals("All"))
            //{
            //    var dt2 = _nhap.GetAll().Where(j =>  (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList();
            //    _list = dt2;
            //    return Json(dt2);
            //}

            //TempData["dt"] = _list;

            return Json(dt);
        }

        public IActionResult PrintMdh(string mdh)
        {
            var dt = _nhap.GetAll().Where(j =>j.IsActive  && j.MaDonHang.Equals(mdh)).ToList();
            return Json(dt);
        }
        public IActionResult GetMaDonhangOrtenHangs()
        {
            var t = new List<string>();
            var tenhangs = _nhap.GetAll().Select(j => j.TenHang).Distinct().ToList();
            var madh = _nhap.GetAll().Select(j => j.MaDonHang).Distinct().ToList();
            t.AddRange(tenhangs);
            t.AddRange(madh);
            return Json(t);

        }
        public IActionResult OnClickmaDh([FromBody] NhapHangRequet requet)
        {

            var dt = _nhap.GetAll().Where(j =>  (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive));
             if(requet.Name.Equals("0")) dt = _nhap.GetAll().ToList().FindAll(j =>j.MaDonHang.Equals(requet.MaDh)&& (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive));
            if (requet.Name.Equals("1")) dt = _nhap.GetAll().ToList().FindAll(j => j.TenHang.Equals(requet.MaDh) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive));
            return Json(dt.ToList());
        }
        public JsonResult GetNxs()
        {
            return Json(_qlNx.GetAll(IdCty()).Where(j => j.IsActive ==true).ToList());
        }
        public JsonResult GetAll(string ncc, string start, string end)
        {
            //return Json(_nhap.GetAll(IdCty()));
            return Json("");
        }

        public IActionResult OnDelete(string id)
        {
            var idcty = IdCty();
           
            var kh = _qlNx.GetNhap(idcty, id);
            if (kh != null)
            {
                kh.IsActive = false;
                _qlNx.Update(kh);
            }
            var dt = _nhap.GetAll().Where(j=> j.MaDonHang == id && j.IdCty == idcty).ToList();
            foreach (var w in dt)
            {
                w.IsActive = false;
              
            }
            _nhap.CreateOrUpdates(dt);
          

            return Content(id);
        }

        [HttpPost]
        public IActionResult Edit1(string mdh)
        {
            var kt = _nhap.GetAll().Where(j => j.MaDonHang.Equals(mdh) && j.IsActive == true && j.TenHang!= null).ToList();

            var qlnx = _qlNx.GetNhap(IdCty(), mdh);

            if (qlnx != null)
            {
                var t = new NhapHang()
                {
                    MaDonHang = qlnx.MaDonHang,
                    DonGiaMua = qlnx.ThanhToan,
                    SoLuong = (int)qlnx.ThanhTien,
                    Id = qlnx.Id
                };
                kt.Add(t);
            }
            
            return Json(kt);
         
        }
        [HttpPost]
        public JsonResult GetTenNcc()
        {
            var ncc = _ncc.GetAll().ToList().Select(j => j.TenNcc).Distinct().ToList();
            ViewBag.nccs = ncc;
            return Json(ncc);
        }
        public JsonResult GetTenHang()
        {
            var dt = _nhap.GetAll().Select(j => j.TenHang).Distinct();
            return Json(dt);
        }
        [HttpPost]
        public IActionResult Details(string id)
        {
            return RedirectToActionPermanent("Details","DetailNhapHang");
        }

        //---------------------------- download 
        public async Task<IActionResult> OnPostExport(string filename)
        {

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = String.Format("Download/{0}.xlsx", filename);
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));


            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(filename);
                IRow row = excelSheet.CreateRow(1);

                row.CreateCell(0).SetCellValue("STT");
                row.CreateCell(1).SetCellValue("Đơn Hàng");
                row.CreateCell(2).SetCellValue("Số Lượng");
                row.CreateCell(3).SetCellValue("Đơn Giá");
                row.CreateCell(4).SetCellValue("Thành Tiền");
                row.CreateCell(5).SetCellValue("Ngày Ghi");
                int dem = 2;
                var ls = TempData["dt"] as List<NhapHang>;
                foreach (var q in ls)
                {
                    dem++;
                    row = excelSheet.CreateRow(dem);
                    row.CreateCell(0).SetCellValue(q.MaDonHang);
                    row.CreateCell(1).SetCellValue(q.TenHang);
                    row.CreateCell(2).SetCellValue(q.SoLuong + "(" + q.Dvt + ")");
                    row.CreateCell(3).SetCellValue(q.DonGiaMua.ToString("##,###,###,###"));
                    row.CreateCell(4).SetCellValue((q.DonGiaMua * q.SoLuong).ToString("##,###,###,###"));
                    row.CreateCell(5).SetCellValue(q.NgayGhi.ToString("d"));
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var file1 = File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            return Content(file1.FileDownloadName);
        }
    }
}