using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartDal _cartDal;
        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;
        }
        public IResult Add(CartItem cartItem)
        {
            _cartDal.Add(cartItem);
            return new SuccessResult("Sepete Eklendi");
        }

        public IResult Delete(CartItem cartItem)
        {
            _cartDal.Delete(cartItem);
            return new SuccessResult("Sepetten Silindi");
        }

        public IDataResult<CartItem> GetById(int cartItemId)
        {
            return new SuccessDataResult<CartItem>(_cartDal.Get(c => c.CartItemId == cartItemId));
        }

        public IDataResult<List<CartItem>> GetList()
        {
            return new SuccessDataResult<List<CartItem>>(_cartDal.GetList().ToList());
        }

        public IDataResult<List<CartItem>> GetListByProduct(int productId)
        {
            return new SuccessDataResult<List<CartItem>>(_cartDal.GetList(p => p.ProductId == productId).ToList());
        }

        public IResult Update(CartItem cartItem)
        {
            _cartDal.Update(cartItem);
            return new SuccessResult("Sepet Güncellendi");
        }

    }
}
