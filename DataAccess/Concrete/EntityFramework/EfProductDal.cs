using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Entities.DTOs.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, ProfessionalProjectContext>, IProductDal
    {
        private readonly ProfessionalProjectContext _context;

        public EfProductDal(ProfessionalProjectContext context)
        {
            _context = context;
        }

        public List<ProductDetailDto> GetProducts()
        {
            return _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDetailDto
                {
                    ProductName = p.ProductName,
                    CategoryName = p.Category.CategoryName
                    
                })
                .ToList();
        }
    }
}
