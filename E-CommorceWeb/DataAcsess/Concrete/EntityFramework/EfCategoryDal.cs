using Core.DataAcsess.EntityFramework;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsess.Concrete.EntityFramework
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, MySQLContext>, ICategoryDal
    {
    }
}
