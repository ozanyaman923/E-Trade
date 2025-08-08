using AutoMapper;
using AutoMapper.QueryableExtensions;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcern.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using Entities.DTOs.Product;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
        IMapper _mapper;
        ProfessionalProjectContext _context;

        public ProductManager(IProductDal productDal,ICategoryDal categoryDal, IMapper mapper, ProfessionalProjectContext context)
        {
            _productDal = productDal;
            _categoryDal = categoryDal;
            _mapper = mapper;
            _context = context;

        }
      

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(ProductCreateDto dto)

        {

            IResult result = BusinessRules.Run(CheckAdded(dto.ProductName));
            if(result != null)
            {
                return result;
            }
            _productDal.Add(dto);
            return new SuccessResult(Messages.ProductAdded);
        }   
        public IDataResult <List<ProductDetailDto>> GetProducts()
        {

            var productsFromDb = _context.Products.ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider).ToList();

            List<ProductDetailDto> productDTOs = _mapper.Map<List<ProductDetailDto>>(productsFromDb);

            return new SuccessDataResult<List<ProductDetailDto>>(productDTOs,Messages.ProductsListed);
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

        public IDataResult<List<Product>> GetByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p=>p.CategoryId==categoryId),Messages.ProductsListed);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p=>p.ProductId==productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min &&  p.UnitPrice<=max ));
        }

        //public IDataResult<List<ProductDetailDto>> GetProductDetails()
        //{
        //    return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetAll(), Messages.ProductsListed);
        //}
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
            //var productToUpdate = _productDal.Get(p => p.ProductId == dto.ProductId);
            //if(productToUpdate == null)
            //{
            //    return new ErrorResult(Messages.ProductNotFound);
            //}
            //productToUpdate.UnitPrice = dto.UnitPrice;
            //productToUpdate.UnitsInStock = dto.UnitsInStock;
            //productToUpdate.ProductName = dto.ProductName;
            //productToUpdate.CategoryId = dto.CategoryId;

            //_productDal.Update(productToUpdate);
            return new SuccessResult(Messages.ProductUpdated);
        }

       
    }
}
