using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE160244.ProductManagement.API.Contract.request;
using SE160244.ProductManagement.Repo.Models;
using SE160244.ProductManagement.Repo.Repositories.Interface;
using System.Linq.Expressions;

namespace SE160244.ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public CategoriesController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet("GetAllCategory")]
        public ActionResult<IEnumerable<Category>> GetAllCategories([FromQuery] QueryParameters queryParams)
        {
            Expression<Func<Category, bool>> filter = null;
            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null;

            if (!string.IsNullOrEmpty(queryParams.OrderBy))
            {
                if (queryParams.SortOrder == SortOrder.Ascending)
                {
                    orderBy = q => q.OrderBy(e => EF.Property<object>(e, queryParams.OrderBy));
                }
                else if (queryParams.SortOrder == SortOrder.Descending)
                {
                    orderBy = q => q.OrderByDescending(e => EF.Property<object>(e, queryParams.OrderBy));
                }
            }

            var entities =  _uow.CategoryRepository.Get(
                filter: filter,
                orderBy: orderBy,
                includeProperties: "", // No includes by default
                page: queryParams.PageNumber,
                size: queryParams.PageSize
            );

            return Ok(entities);
        }

        [HttpPost("GetCategoryById/{id}")]
        public ActionResult GetCategoryById(int id)
        {
            var category =  _uow.CategoryRepository.GetByID(id);
            if(category == null) {
                return BadRequest();
            }
            return Ok(category);
        }

        [HttpPost("InsertCategory")]
        public ActionResult  InserCategory([FromForm]CreateCategoryRequest request)
        {
            var category = new Category
            {
                CategoryName = request.CategoryName,
            };

             _uow.CategoryRepository.Insert(category);
            return Ok(category);
        }

        [HttpPut("UpdateCategory/{id}")]
        public ActionResult UpdateCategory([FromQuery]UpdateCategoryRequest request, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result =  _uow.CategoryRepository.GetByID(id);
            if(result == null)
            {
                return NotFound();
            }

            result.CategoryName = request.CategoryName;

            _uow.CategoryRepository.Update(result);
            return Ok(result);

        }
        [HttpDelete("DeleteCategory/{id}")]
        public ActionResult DeleteCategory(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingcategory =  _uow.CategoryRepository.GetByID(id);
            if (existingcategory == null)
            {
                return NotFound(new { Message = $"Product with id {id} not found." });
            }

             _uow.CategoryRepository.Delete(existingcategory);
            return Ok(new { Message = $"Category with id {id} has been deleted." });
        }
    }
}
