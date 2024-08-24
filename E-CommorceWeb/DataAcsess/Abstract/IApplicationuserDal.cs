using Core.DataAcsess;
using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsess.Abstract
{
    public interface IApplicationuserDal : IEntityRepository<ApplicationUser>
    {
    }
}
