using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Xn.Models;

namespace Xn.Services
{
   public class NhapService:DomainService,INhapService
   {
       private readonly IGetIdService _getIdService;
       private readonly IRepository<NhapHang> _repository;
        public NhapService(IGetIdService getIdService, IRepository<NhapHang> repository)
        {
            _getIdService = getIdService;
            _repository = repository;
        }
        public IEnumerable<NhapHang> GetAll()
        {
            var list = _repository.GetAllList().FindAll(j => j.CreateUserId.Equals(_getIdService.CreateIdUser()) && j.TenHang!="");
            return list;
        }

        public IEnumerable<NhapHang> GetAll(string ncc, DateTime begin, DateTime end)
        {
            var list = _repository.GetAllList().FindAll(j => j.CreateUserId.Equals(_getIdService.CreateIdUser()) && j.NgayGhi>= begin && j.NgayGhi<= end && j.TenHang != "");
            return list;
        }

        public NhapHang GetByMaDh(string madonhang)
        {
            var list = _repository.GetAllList().Find(j => j.CreateUserId.Equals(_getIdService.CreateIdUser()) && j.MaDonHang.Equals(madonhang) && j.TenHang != "");
            return list;
        }

        public void Create(NhapHang entity)
        {
            _repository.Insert(entity);
        }

        public void Update(NhapHang entity)
        {
            _repository.Update(entity);
        }

        public void Delete(int id)
        {
           _repository.Delete(id);
        }

        public void DeleteMaDh(string madh)
        {
            var list = _repository.GetAllList().FindAll(j => j.CreateUserId.Equals(_getIdService.CreateIdUser()) && j.MaDonHang.Equals(madh));
            foreach (var w in list)
            {
                Delete(w.Id);
            }
        }

       public void CreateOrUpdate(NhapHang entity)
       {
           _repository.InsertOrUpdate(entity);
       }

       public void CreateOrUpdates(List<NhapHang> entity)
       {
           foreach (var w in entity)
           {
               _repository.InsertOrUpdate(w);
           }
       }
   }
}
