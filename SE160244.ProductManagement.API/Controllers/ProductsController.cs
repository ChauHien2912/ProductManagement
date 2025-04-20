using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE160244.ProductManagement.API.Contract.request;
using SE160244.ProductManagement.Repo.Models;
using SE160244.ProductManagement.Repo.Repositories.Interface;
using System.Linq.Expressions;
using System.Security.Cryptography.Xml;

namespace SE160244.ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ProductsController(IUnitOfWork uow)
        {
            _uow = uow;
        }


            [HttpGet("GetAllProduct")]
            public ActionResult<IEnumerable<Product>> GetProducts([FromQuery]QueryParameters query)
            {
                Expression<Func<Product, bool>> filter = null;
            

                Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null;

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    if (query.SortOrder == SortOrder.Ascending)
                    {
                        orderBy = q => q.OrderBy(e => EF.Property<object>(e, query.OrderBy));
                    }
                    else if (query.SortOrder == SortOrder.Descending)
                    {
                        orderBy = q => q.OrderByDescending(e => EF.Property<object>(e, query.OrderBy));
                    }
                }

                string[] includeProperties = null;
                if (!string.IsNullOrEmpty(query.IncludeProperties))
                {
                    includeProperties = query.IncludeProperties.Split(',');
                }

                var entities =  _uow.ProductRepository.Get(
                    filter: filter,
                    orderBy: orderBy,
                    includeProperties: "", // No includes by default
                    page: query.PageNumber,
                    size: query.PageSize
                );

                return Ok(entities);
            }

        [HttpPost("GetProductById/{id}")]
        public  ActionResult GetProductById(int id)
        {
            var product = _uow.ProductRepository.GetByID(id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }

        }


        [HttpPost("InsertProduct")]
        public  ActionResult InsertProduct(CreateProductRequest request)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                UnitsinStock = request.UnitsinStock,
                UnitPrice = request.UnitPrice,
                CategoryId = request.CategoryId,
            };

            _uow.ProductRepository.Insert(product);
            return Ok(product);
        }

        [HttpPut("UpdateProduct/{id}")]
        public ActionResult UpdateProduct([FromForm] UpdateProductRequest request, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = _uow.ProductRepository.GetByID(id);
            if (existingProduct == null)
            {
                return NotFound(new { Message = $"Product with id {id} not found." });
            }

            // Update properties
            existingProduct.ProductName = request.ProductName;
            existingProduct.UnitsinStock = request.UnitsinStock;
            existingProduct.UnitPrice = request.UnitPrice;
            existingProduct.CategoryId = request.CategoryId;

            // Call update method (assuming it returns void)
             _uow.ProductRepository.Update(existingProduct);

            return Ok(existingProduct);
        }
        
        [HttpDelete("DeleteProduct/{id}")]
        public  ActionResult DeleteProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct =  _uow.ProductRepository.GetByID(id);
            if (existingProduct == null)
            {
                return NotFound(new { Message = $"Product with id {id} not found." });
            }

             _uow.ProductRepository.Delete(existingProduct);
            return Ok(new { Message = $"Product with id {id} has been deleted." });
        }

    }
}
