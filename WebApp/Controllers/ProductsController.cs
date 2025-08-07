using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProductsController : Controller
    {
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetId")]
        public IActionResult GetId(int productId)
        {
            

            var result = _productService.GetById(productId);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpGet("GetCategoryId")]
        public IActionResult GetAll(int categoryId)
        {
           
       

            var result = _productService.GetByCategoryId(categoryId);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            //Swagger
            //Dependency chain --

            Thread.Sleep(1000);

            var result = _productService.GetAll();
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpPost("add")]
        public IActionResult Add([FromBody]Product product)
        {
            var result = _productService.Add(product);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        
        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _productService.DeleteById(id);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("update")]
        public IActionResult Update(ProductUpdateDto dto)
        {
            var result = _productService.Update(dto);
            if(result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


    }
}
