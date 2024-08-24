using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAcsess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class LoginManager : ILoginService
    {
        private ILoginDal _loginDal;
        public LoginManager(ILoginDal loginDal)
        {
            _loginDal = loginDal;
        }
        public IResult Add(ApplicationUser applicationUser)
        {
            _loginDal.Add(applicationUser);
            return new SuccessResult(Messages.loginAdded);
        }

        public IResult Delete(ApplicationUser applicationUser)
        {
            _loginDal.Delete(applicationUser);
            return new SuccessResult(Messages.loginDeleted);
        }

        public IDataResult<ApplicationUser> GetById(int applicationUserId)
        {
            return new SuccessDataResult<ApplicationUser>(_loginDal.Get(l=> l.ApplicationUserId == applicationUserId));
        }

        public IDataResult<List<ApplicationUser>> GetList()
        {
            return new SuccessDataResult<List<ApplicationUser>>(_loginDal.GetList().ToList());
        }

        public IResult Update(ApplicationUser applicationUser)
        {
            _loginDal.Update(applicationUser);
            return new SuccessResult(Messages.loginUpdated);
        }
    }
}
