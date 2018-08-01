using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Services;
using Xn.Authorization.Users;

namespace Xn.Services
{
    public class GetIdService : DomainService, IGetIdService
    {
        private readonly UserManager _userManager;
        private readonly ICompanyService _companyService;
        public GetIdService(UserManager userManager, ICompanyService companyService)
        {
            _userManager = userManager;
            _companyService = companyService;
        }
        public long CreateIdUser()
        {
            var check = _userManager.AbpSession.UserId;
            return check != null ? check.Value : 0;
        }

        public long IdCty()
        {
            var check = _userManager.AbpSession.UserId;
            var company = _companyService.GetAll().FirstOrDefault(k => k.CreateIdUser.Equals(check.Value));
            return company != null ? company.Id : 0;
        }
    }
}
