using Core.DataAcsess;
using Core.DataAcsess.EntityFramework;
using Core.Entities;
using DataAcsess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsess.Abstract
{
    public interface ILoginDal : IEntityRepository<ApplicationUser>
    {
    }
}
