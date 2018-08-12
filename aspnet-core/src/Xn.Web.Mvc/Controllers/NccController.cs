using System;
using System.Globalization;
using System.Linq;
using Abp.AutoMapper;
using AutoMapper;
using Functions;
using Microsoft.AspNetCore.Mvc;
using Xn.Authorization.Users;
using Xn.Controllers;
using Xn.Models;
using Xn.Services;
using Xn.Users;
using Xn.Web.Models;
using Xn.Web.Models.NccEntity;


namespace Xn.Web.Controllers
{
    public class NccController : XnControllerBase
    {
        private INccService _nccService;
        private ICompanyService _companyService;
        private UserAppService _appService;
        private UserManager _userManager;
        private IGetIdService _getIdService;
        private long _idCty;

        public NccController(INccService nccService, ICompanyService companyService, UserAppService appService, UserManager userManager, IGetIdService getIdService)
        {
           
            _nccService = nccService;
            _companyService = companyService;
            _appService = appService;
            _userManager = userManager;
            _getIdService = getIdService;
            _idCty = (long)_userManager.AbpSession.UserId;
        }
        public IActionResult Index()
        {
            _idCty = (long)_userManager.AbpSession.UserId;
            return View();
        }

        private int IdCty()
        {
            _idCty = (long)_userManager.AbpSession.UserId;
            return (int) _idCty;
        }
        [HttpPost]
        public IActionResult GetAll()
        {
          
            var dt = _nccService.GetAllbtIdCty(IdCty());
            return Json(dt);
        }

        public IActionResult GetNameNccs()
        {
            var dt = _nccService.GetAll().Select(j => j.TenNcc).Distinct().ToList();
            return Json(dt);
        }

       

        public IActionResult GetId()
        {
            return Json(IdCty());
        }
        [HttpPost]
        public IActionResult CreateOrEdit([FromBody]NccEntity entity)
        {
            var tt = "";
            var check = _nccService.GetAll().FirstOrDefault(j => j.TenNcc.Equals(entity.TenNcc));
            if (check != null)
            {
                return Json("Đã có tên nhà cung cấp này , tạo mới thất bại");
            }
            else
            {
                entity.CreateUserId = _getIdService.CreateIdUser();

               
                // insert
                var ouput = entity.MapTo<Ncc>();
                ouput.NgayGhi = Fn.ParseDate(entity.NgayGhi);
                ouput.IsActive = true;
                ouput.Code = entity.MasoThue;
                ouput.KeyUser = _appService.AbpSession.UserId.ToString();
                ouput.NgaySinh = DateTime.Today;
                ouput.IdCty = (int)IdCty();
                ouput.TenNcc = entity.TenNcc;
                if (ModelState.IsValid)
                {
                    _nccService.CreateOrUpdate(ouput);
                }
            }
          
           
            return Json(tt);
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            var t = _nccService.GetById(id);
            
            return t != null ? Json(t.MapTo<NccEntity>()) : Json("0");
        }

        public IActionResult Delete(int id)
        {
            _nccService.Delete(id);
            return Json("Đã Xóa Xong Dữ Liệu");

        }

        public IActionResult CreateEdit()
        {
            return View();
        }

        public IActionResult CheckTenNcc(string data)
        {
            try
            {
                var check = _nccService.GetAll().ToList().Find(j => j.TenNcc.ToLower().Equals(data.ToLower()));
                return Json(check != null ? "1" : "0");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}