using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Services;
using Xn.Models;

namespace Xn.Services
{
   public interface INhapService:IDomainService
   {
       IEnumerable<NhapHang> GetAll();
       IEnumerable<NhapHang> GetAll(string ncc, DateTime begin, DateTime end);
       NhapHang GetByMaDh(string madonhang);
       void Create(NhapHang entity);
       void Update(NhapHang entity);
       void Delete(int id);
       void DeleteMaDh(string madh);
       void CreateOrUpdate(NhapHang entity);
       void CreateOrUpdates(List<NhapHang> entity);
    }
}
