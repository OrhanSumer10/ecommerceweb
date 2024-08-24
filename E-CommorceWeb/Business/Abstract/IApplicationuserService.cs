using Core.Utilities.Results;
using DataAcsess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IApplicationuserService 
    {
        IDataResult<ApplicationUser> GetById(int applicationUserId);

        IDataResult<List<ApplicationUser>> GetList();

        IResult Add(ApplicationUser applicationUser);
        IResult Delete(ApplicationUser applicationUser);
        IResult Update(ApplicationUser applicationUser);
    }
}
