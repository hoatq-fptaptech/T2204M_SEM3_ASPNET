using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2204M_API.Entities;
using T2204M_API.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace T2204M_API.Controllers
{
    [ApiController]
    [Route("api/category")]
    [Authorize(Roles = "user")]
    public class CategoryController : ControllerBase
    {
        private readonly T2204mApiContext _context;
        //Scaffold-DbContext "Data Source=localhost,1433; Database=T2204M_API;User Id=sa;Password=sa123456;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Force
        public CategoryController(T2204mApiContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var categories = _context.Categories.Include(c=>c.Products).ToList();
            List<CategoryDTO> list = new List<CategoryDTO>();
            foreach(var item in categories)
            {
                List<ProductDTO> plist = new List<ProductDTO>();
                foreach(var p in item.Products)
                {
                    plist.Add(new ProductDTO { id = p.Id, name = p.Name,price=p.Price,description=p.Description });
                }
                list.Add(new CategoryDTO {id=item.Id,name=item.Name,products=plist});
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            var category = _context.Categories.Find(id);
            if(category != null) {
                return Ok(new CategoryDTO { id = category.Id, name = category.Name });     
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Policy = "SuperAdmin")]
        public IActionResult Create(CategoryDTO data)
        {
            if (ModelState.IsValid)
            {
                var category = new Category { Name = data.name };
                _context.Categories.Add(category);
                _context.SaveChanges();
                return Created($"get-by-id?id={category.Id}",new CategoryDTO { id=category.Id,name=category.Name});
            }
            return BadRequest();
        }

        [HttpPut]
        [Authorize(Policy = "SuperAdmin")]
        public IActionResult Update(CategoryDTO data)
        {
            if (ModelState.IsValid)
            {
                var category = new Category { Id = data.id, Name = data.name };
                _context.Categories.Update(category);
                _context.SaveChanges();
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Policy = "SuperAdmin")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

