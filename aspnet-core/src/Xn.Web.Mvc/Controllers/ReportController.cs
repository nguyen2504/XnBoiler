using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Extensions;
using FastMember;
using Functions;
using MF.Dev.ExportHelper.ExportHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resto.Framework.UI.Common.ExcelExport;
using Xn.Controllers;
using Xn.Models;
using Xn.Services;
using Xn.Web.Models;
using PropertyDescriptor = Castle.Components.DictionaryAdapter.PropertyDescriptor;

namespace Xn.Web.Controllers
{
    public class ReportController : XnControllerBase
    {
        private readonly INhapHangService _nhap;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ICompanyService _congty;
        private List<NhapHang> _nhapHangs;
        private string _all = "All";
        List<NhapHang> _list = new List<NhapHang>();
        public ReportController(INhapHangService nhap, IHostingEnvironment hostingEnvironment, ICompanyService congty)
        {
            _nhap = nhap;
            _hostingEnvironment = hostingEnvironment;
            //_nhapHangs = new List<NhapHang>();
            _congty = congty;
        }
        public IActionResult Index()
        {
            
            var tenhangs = _nhap.GetAll().Select(j => j.TenHang).Distinct().ToList();
            var madh = _nhap.GetAll().Select(j => j.MaDonHang).Distinct().ToList();
            var ncc = _nhap.GetAll().Select(k => k.TenNcc).Distinct().ToList();
            var t = new ReportsIndex() { TenNccs = ncc, TenHangs = tenhangs, DonHangs = madh };
            ViewBag.nccs = t;
            return View(new ReportsIndex(){TenNccs = ncc,DonHangs = madh, TenHangs = tenhangs});
        }

     
        [HttpPost]
        public IActionResult Index(string mdh)
        {
            return Json("");
        }
        [HttpPost]
        public JsonResult Ins(string mdh)
        {
             _nhapHangs = GetHangs(mdh);
            return Json(_nhapHangs);
        }
        // data 
        public List<NhapHang> GetHangs(string mdh)
        {
            var dt = _nhap.GetAll().Where(j => j.IsActive).OrderByDescending(j => j.NgayGhi).ToList();
            return mdh != null ? dt.FindAll(j => j.MaDonHang.Equals(mdh)) : dt;
        }

      
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                System.ComponentModel.PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        //0
        public IActionResult OnTenHang([FromBody] NhapHangRequet requet)
        {
            var dt = _nhap.GetAll().Where(j =>j.TenHang.Equals(requet.Name) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList();            
            if (requet.Name == _all) dt = All(requet);
            //_list = dt;
            //TempData["dta"] = dt;
            return Json(dt);
        }
        
        public List<NhapHang> All([FromBody] NhapHangRequet requet)
        {
           return _nhap.GetAll().Where(j => (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
        }
        //1
        public IActionResult OnDonHang([FromBody] NhapHangRequet requet)
        {
            var dt = _nhap.GetAll().Where(j => j.MaDonHang.Equals(requet.Name) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
            if (requet.Name == _all) dt = All(requet);
           //_list = dt;
            //TempData["dta"] = dt;
            return Json(dt);
        }
        //2
        public IActionResult OnNcc([FromBody] NhapHangRequet requet)
        {
            var dt = _nhap.GetAll().Where(j => j.TenNcc.Equals(requet.Name) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
            if (requet.Name == _all) dt = All(requet);
             //_list = dt;
            //TempData["dta"] = dt;
            return Json(dt);
        }
        //3
        public IActionResult OnSearch([FromBody] NhapHangRequet requet)
        {
            if (requet.MaDh == null) requet.MaDh = _all;
            var dt = _nhap.GetAll().Where(j => 
            (j.TenNcc.ToLower().Contains(requet.Name.ToLower())
            || j.TenHang.ToLower().Contains(requet.Name.ToLower())
            || j.MaDonHang.ToLower().Contains(requet.Name.ToLower())) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
            if (requet.Name == _all) dt = All(requet);
            //  _list = dt;
            //TempData["dta"] = dt;
            return Json(dt);
        }

        public List<NhapHang> GetData([FromBody] NhapHangRequet requet)
        {
            List<NhapHang> dt = new List<NhapHang>();
            if (requet.Memory.Equals("-1"))
            {
                dt = _nhap.GetAll().Where(j => (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList();
                if (requet.Name == _all) dt = All(requet);
            }
            if (requet.Memory.Equals("0"))
            {
                dt = _nhap.GetAll().Where(j => j.TenHang.Equals(requet.Name) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList();
                if (requet.Name == _all) dt = All(requet);
            }
            if (requet.Memory.Equals("1"))
            {
                dt = _nhap.GetAll().Where(j => j.MaDonHang.Equals(requet.Name) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
                if (requet.Name == _all) dt = All(requet);
            }
            if (requet.Memory.Equals("2"))
            {
                dt = _nhap.GetAll().Where(j => j.TenNcc.Equals(requet.Name) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
                if (requet.Name == _all) dt = All(requet);
            }
            if (requet.Memory.Equals("3"))
            {
                if (requet.MaDh == null) requet.MaDh = _all;
             dt = _nhap.GetAll().Where(j =>
                    (j.TenNcc.ToLower().Contains(requet.Name.ToLower())
                     || j.TenHang.ToLower().Contains(requet.Name.ToLower())
                     || j.MaDonHang.ToLower().Contains(requet.Name.ToLower())) && (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
                if (requet.Name == _all) dt = All(requet);
            }
            return dt;
        }

        public IActionResult GetSearching([FromBody] NhapHangRequet requet)
        {
            var dt = _nhap.GetAll().Where(j =>  (j.NgayGhi >= Fn.StartHour(Fn.ParseDate(requet.Begin)) && Fn.EnDhour(Fn.ParseDate(requet.End)) >= j.NgayGhi && j.IsActive)).ToList(); ;
            if (requet.Name == _all) dt = All(requet);
            //_list = dt;
            //TempData["dta"] = dt;
            return Json(dt);
        }
        public async Task<IActionResult> OnPostExport([FromBody] NhapHangRequet requet)
        {

            var filename = requet.Name+"-"+Fn.GetDateReport(requet.Begin)+"."+ Fn.GetDateReport(requet.End);
            if (Fn.GetDateReport(requet.Begin) == Fn.GetDateReport(requet.End))
            {
                filename = requet.Name + "-" + Fn.GetDateReport(requet.Begin) ;
            }
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName =String.Format("Download/{0}.xlsx", filename);
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

     
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(filename);
                ISheet sheet1 = workbook.CreateSheet("Sheet1");

                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                var rowIndex1 = 0;
                IRow row1 = sheet1.CreateRow(rowIndex1);
                row1.Height = 30 * 80;
                row1.CreateCell(0).SetCellValue("this is content");
                sheet1.AutoSizeColumn(0);
                rowIndex1++;

                var sheet2 = workbook.CreateSheet("Sheet2");
                var style1 = workbook.CreateCellStyle();
                style1.FillForegroundColor = HSSFColor.Blue.Index2;
                style1.FillPattern = FillPattern.SolidForeground;

                var style2 = workbook.CreateCellStyle();
                style2.FillForegroundColor = HSSFColor.Yellow.Index2;
                style2.FillPattern = FillPattern.SolidForeground;

                var cell2 = sheet2.CreateRow(0).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(0);

                cell2 = sheet2.CreateRow(1).CreateCell(0);
                cell2.CellStyle = style2;
                cell2.SetCellValue(1);

                //workbook.Write(fs);
                IRow row = excelSheet.CreateRow(1);

                row.CreateCell(0).SetCellValue("STT");
                row.CreateCell(1).SetCellValue("Đơn Hàng");
                row.CreateCell(2).SetCellValue("Tên Hàng");
                row.CreateCell(3).SetCellValue("Số Lượng");
                row.CreateCell(4).SetCellValue("Đơn Giá");
                row.CreateCell(5).SetCellValue("Thành Tiền");
                row.CreateCell(6).SetCellValue("Ngày Ghi");
                int dem = 1;
                int dem1 = 10;
                var h = _list;
                var ls = GetData(requet);
                //var hhh = ToDataTable(ls);
                //DataTable table = ToDataTable(ls); // Get the data table.
                //foreach (DataRow d in table.Rows) // Loop over the rows.
                //{
                //    dem1++;
                //    row = excelSheet.CreateRow(dem1);
                //    var j = 0;
                //    foreach (var item in d.ItemArray) // Loop over the items.
                //    {
                //        row.CreateCell(j++).SetCellValue(item.ToString());
                //    }
                //}
                var stt = 0;
                foreach (var q in ls)
                    {
                        dem++;
                        row = excelSheet.CreateRow(dem);
                        row.CreateCell(0).SetCellValue(stt++);
                        row.CreateCell(1).SetCellValue(q.MaDonHang);
                        row.CreateCell(2).SetCellValue(q.TenHang);
                        row.CreateCell(3).SetCellValue(q.SoLuong + "(" + q.Dvt + ")");
                        row.CreateCell(4).SetCellValue(q.DonGiaMua.ToString("##,###,###,###"));
                        row.CreateCell(5).SetCellValue((q.DonGiaMua * q.SoLuong).ToString("##,###,###,###"));
                        row.CreateCell(6).SetCellValue(q.NgayGhi.ToLocalTime().ToShortDateString());
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

        public IActionResult CongTy()
        {
            var cty = _congty.GetAll().ToList();
            return Json(cty);
        }
      
    }
}