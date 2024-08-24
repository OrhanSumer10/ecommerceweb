using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICartService
    {
        IDataResult<CartItem> GetById(int cartItemId);

        IDataResult<List<CartItem>> GetList();
        IDataResult<List<CartItem>> GetListByProduct(int productId);
        IResult Add(CartItem cartItem);
        IResult Delete(CartItem cartItem);
        IResult Update(CartItem cartItem);

    }
}
