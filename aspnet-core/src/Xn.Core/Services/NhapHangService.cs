using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Xn.Authorization.Users;
using Xn.Models;

namespace Xn.Services
{
   public class NhapHangService:DomainService,INhapHangService
   {
       private readonly IRepository<NhapHang> _repository;
       private readonly IGetIdService _getIdService;
       public NhapHangService(IRepository<NhapHang> repository, IGetIdService getIdService)
       {
           _repository = repository;
           _getIdService = getIdService;
       }
        public IEnumerable<NhapHang> GetAll()
        {
            return _repository.GetAll().Where(k => k.CreateUserId.Equals(_getIdService.CreateIdUser()));
        }

        public IEnumerable<NhapHang> GetAll(int idcty)
        {
            return _repository.GetAll().Where(j => j.IdCty.Equals(idcty) && j.CreateUserId.Equals(_getIdService.CreateIdUser())).OrderByDescending(j => j.Id).ToList();
        }

       public NhapHang GetById(int id)
       {
           return _repository.FirstOrDefault(id);
       }

       public async Task<NhapHang> Create(NhapHang entity)
       {
           entity.CreateUserId = _getIdService.CreateIdUser();
           var check =await _repository.InsertAsync(entity);
           return check;
       }

        public async void Creates(IList<NhapHang> entitys)
        {
            foreach (var w in entitys)
            {
                w.CreateUserId = _getIdService.CreateIdUser();
                await _repository.InsertAsync(w);
            }
          
        }

       public void CreatesOrUpdates(IList<NhapHang> entitys)
       {
           foreach (var w in entitys)
           {
               if (w.Id == 0)
               {
                   _repository.Insert(w);
               }
               else
               {
                   _repository.Update(w);
               }
             
           }
        }


       public void Update(IList<NhapHang> entitys)
       {
           foreach (var w in entitys)
           {
               _repository.Update(w);
           }
       }

       public void UpdateId(NhapHang entity)
       {
           _repository.Update(entity);
        }

       public void Update(NhapHang entity)
        {
            _repository.Update(entity);
        }

      

       public void Delete(int idcty)
        {
           _repository.Delete(idcty);
        }

       public void DeleteMaDh(IList<QlNcc> entitys)
       {
           foreach (var w in entitys)
           {
              _repository.Delete(w.Id);
           }
       }


       public IEnumerable<NhapHang> Search(string ncc, DateTime begin, DateTime end)
       {
           return _repository.GetAllList()
               .FindAll(j => j.TenNcc.Contains(ncc) && j.NgayGhi >= begin && j.NgayGhi <= end);
       }

       public IEnumerable<string> TenHangs(int idcty)
       {
            return _repository.GetAllList().ToList().Select(j => j.TenHang).Distinct();
       }

       public IEnumerable<string> TenNccs(int idcty)
       {
            return _repository.GetAllList().ToList().Select(j => j.TenNcc).Distinct();
        }

        public void DeleteMaDh(IList<NhapHang> entitys)
        {
            throw new NotImplementedException();
        }
    }
}
