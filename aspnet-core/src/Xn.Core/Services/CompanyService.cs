using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Xn.Authorization.Users;
using Xn.Models.Company;

namespace Xn.Services
{
   public class CompanyService:DomainService, ICompanyService
   {
     
        private readonly IRepository<Company> _repository;
       private readonly UserManager _userManager;

        public CompanyService(IRepository<Company> repository, UserManager userManager)
       {
           _repository = repository;
           _userManager = userManager;

        }

       public IEnumerable<Company> GetAll()
       {
           var id = _userManager.AbpSession.UserId;
           var l = _repository.GetAllList().Where(j => j.CreatorUserId.Equals(id)).ToList();
           return l;
       }

       public Company GetById(string code)
       {
           return _repository.FirstOrDefault(j => j.Code.Equals(code) && j.CreatorUserId.Equals(_userManager.AbpSession.UserId));
       }

       public Company GetByIds(long? id)
       {
           return _repository.FirstOrDefault(j => j.CreatorUserId == id );
        }
       public Company GetByCreatorUserId(long? creatorUserId)
       {
           //var j1 = GetAll();
            var check = _repository.FirstOrDefault(j => j.CreatorUserId.Equals(creatorUserId) && j.IdCty>0);
            //var check = _repository.GetAllList(k=>k.Id ==1);
            return check;
       }

       //public Company GetByCreatorUserId(long? creatorUserId, string email)
       //{
       //    return _repository.FirstOrDefault(j => j.CreatorUserId == creatorUserId );
       // }

       public async Task<Company> Create(Company entity)
       {
           entity.CreateIdUser = (long) _userManager.AbpSession.UserId;
           return await _repository.InsertAsync(entity);
        }
    
        public void Update(Company entity)
       {

           var check = _repository.FirstOrDefault(j => j.Id.Equals(entity.Id));
           if (check == null)
           {
               throw new UserFriendlyException("No Data");
           }
           else
           {
                check.Name = entity.Name;
                check.Address = entity.Address;
                check.Code = entity.Code;
               check.Fax = entity.Fax;
               check.Phone = entity.Phone;
               check.StartFounding = entity.StartFounding;
                //entity.MapTo(check);
                _repository.UpdateAsync(check);
             
           }
          
       }

       public void Delete(int id)
       {
           var check = _repository.FirstOrDefault(j => j.Id.Equals(id));
           if (check == null)
           {
               throw new UserFriendlyException("No Data");
           }
           else
           {
               check.IsDeleted = false;
               _repository.Delete(check);
           }
        }

       public Company GetByIdCty(int idcty)
       {
           try
           {
               var check = _repository.FirstOrDefault(j => j.IdCty.Equals(idcty));
               return check;
            }
           catch (Exception e)
           {
               return null;
           }
        }

       public void Delete(string code)
        {
            throw new NotImplementedException();
        }
    }
}
