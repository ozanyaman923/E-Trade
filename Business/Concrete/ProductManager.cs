using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryDal _categoryDal;
        public ProductManager(IProductDal productDal, ICategoryDal categoryDal)
        {
            _productDal = productDal;
            _categoryDal = categoryDal;              
        }
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckAdded(product.ProductName));
            if(result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IResult DeleteById(int id)
        {
            Product productDelete = _productDal.Get(p=>p.ProductId==id);
            if(productDelete==null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }

            _productDal.Delete(productDelete);
            return new SuccessResult(Messages.ProductDeleted);
        }
        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<Product> GetById(int productId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            throw new NotImplementedException();
        }
        private IResult CheckAdded(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if(result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
        public IResult Update(ProductUpdateDto dto)
        {
            var productToUpdate = _productDal.Get(p=>p.ProductId==dto.ProductId);
            if(productToUpdate==null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }
            productToUpdate.UnitPrice = dto.UnitPrice;
            productToUpdate.UnitsInStock = dto.UnitsInStock;
            productToUpdate.ProductName = dto.ProductName;
            productToUpdate.CategoryId = dto.CategoryId;

            _productDal.Update(productToUpdate);
            return new SuccessResult(Messages.ProductUpdated);
        }
      
    }
}
