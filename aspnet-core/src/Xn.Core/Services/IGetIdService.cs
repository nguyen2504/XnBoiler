using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Services;

namespace Xn.Services
{
  public  interface IGetIdService:IDomainService
  {
      long CreateIdUser();
      long IdCty();
  }
}
