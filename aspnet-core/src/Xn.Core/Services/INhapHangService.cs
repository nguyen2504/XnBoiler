using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Xn.Models;

namespace Xn.Services
{
   public interface INhapHangService:IDomainService
   {
       IEnumerable<NhapHang> GetAll();
       IEnumerable<NhapHang> GetAll(int idcty);
       NhapHang GetById(int id);
       Task<NhapHang> Create(NhapHang entity);
       void Creates(IList<NhapHang> entitys);
       void CreatesOrUpdates(IList<NhapHang> entitys);
        void Update(IList<NhapHang> entitys);
       void UpdateId(NhapHang entity);

        void Delete(int idcty);
       void DeleteMaDh(IList<NhapHang> entitys);
        IEnumerable<NhapHang> Search(string ncc, DateTime begin, DateTime end);
       IEnumerable<string> TenHangs(int idcty);
       IEnumerable<string> TenNccs(int idcty);
    }
}
