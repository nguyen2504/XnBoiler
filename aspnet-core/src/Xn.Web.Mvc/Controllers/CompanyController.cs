using System;
using Abp.Authorization;
using Abp.Domain.Uow;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Xn.Authorization.Users;
using Xn.Company;
using Xn.Company.Dto;
using Xn.Controllers;
using Xn.Services;
using Xn.Models.Company;
namespace Xn.Web.Controllers
{
    public class CompanyController : XnControllerBase
    {
        private readonly ICompanyAppService _companyAppService;
        private readonly ICompanyService _companyService;
        private readonly UserManager _userManager;

        public CompanyController(ICompanyAppService companyAppService, ICompanyService companyService, UserManager userManager)
        {
            _companyAppService = companyAppService;
            _companyService = companyService;
            _userManager = userManager;
        }

        public long IdUser()
        {
            return (long) _userManager.AbpSession.UserId;
        }
     
        public IActionResult Index()
        {
           
            CompanyDto company = new CompanyDto();
            company = _companyAppService.GetByIDtos(IdUser());
            return View(company);
        }

        public IActionResult Edit()
        {
            Xn.Models.Company.Company company = new Xn.Models.Company.Company();
            company = _companyService.GetByCreatorUserId(IdUser());
            return View(company);
          
        }
        [HttpPost]
        public IActionResult Edit(Xn.Models.Company.Company companyDto)
        {
            
            //companyDto.DeletionTime = DateTime.Now;
            //companyDto.LastModifierUserId = 1;
            //companyDto.LastModificationTime = DateTime.Now;
            //companyDto.StartFounding = DateTime.Now;
            //companyDto.CreationTime = DateTime.Now;
            //companyDto.IdScurity = "1";
            //var output = Mapper.Map<Xn.Models.Company.Company>(companyDto);
            _companyService.Update(companyDto);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _companyService.Delete(id);
            return Json("success");
        }
        //public IActionResult Edit([FromBody] CompanyDto company)
        //{
        //    return Json("");
        //}

        public IActionResult GetCompany()
        {
            var data = _companyAppService.GetByIDtos(IdUser());
            return Json(data);
        }

        public IActionResult Create()
        {
            var c = new Xn.Models.Company.Company()
            {
                DeletionTime = DateTime.Now,
                LastModifierUserId = 1,
                LastModificationTime = DateTime.Now,
                StartFounding = DateTime.Now,
                CreatorUserId = IdUser(),
                CreationTime = DateTime.Now,
                IdScurity = "1",
                IdCty = int.Parse(IdUser().ToString()),
            };
            return View(c);
        }
        [HttpPost]
        [UnitOfWork]
        public IActionResult Create(Xn.Models.Company.Company company)
        {
            //company.LastModificationTime = DateTime.Today;
            //company.CreationTime = DateTime.Today;
            //company.DeletionTime = DateTime.Today;
            //company.LastModifierUserId = 1;
          
            company.CreatorUserId = IdUser();
            if (ModelState.IsValid)
            {
                _companyService.Create(company);
            }

            return RedirectToAction("Index","Home");
        }
    }
}