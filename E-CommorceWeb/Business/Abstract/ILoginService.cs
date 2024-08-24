using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ILoginService
    {
        IDataResult<ApplicationUser> GetById(int applicationUserId);

        IDataResult<List<ApplicationUser>> GetList();

        IResult Add(ApplicationUser applicationUserId);
        IResult Delete(ApplicationUser applicationUserId);
        IResult Update(ApplicationUser applicationUserId);
    }
}
