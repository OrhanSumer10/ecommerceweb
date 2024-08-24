using Business.Abstract;
using Core.Utilities.Results;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ApplicationuserManager : IApplicationuserService
    {
        private IApplicationuserDal _applicationuserDal;
        public ApplicationuserManager(IApplicationuserDal applicationuserDal)
        {
            _applicationuserDal = applicationuserDal;
        }
        public IResult Add(ApplicationUser applicationUser)
        {
            _applicationuserDal.Add(applicationUser);
            return new SuccessResult("Kullanıcı Eklendi");
        }

        public IResult Delete(ApplicationUser applicationUser)
        {
            _applicationuserDal.Delete(applicationUser);
            return new SuccessResult("Kullanıcı Silindi");
        }

        public IDataResult<ApplicationUser> GetById(int applicationUserId)
        {
            return new SuccessDataResult<ApplicationUser>(_applicationuserDal.Get(c => c.ApplicationUserId == applicationUserId));
        }

        public IDataResult<List<ApplicationUser>> GetList()
        {
            return new SuccessDataResult<List<ApplicationUser>>(_applicationuserDal.GetList().ToList());
        }

        public IResult Update(ApplicationUser applicationUser)
        {
            _applicationuserDal.Update(applicationUser);
            return new SuccessResult("Kullanıcı Güncellendi");
        }
    }
}
