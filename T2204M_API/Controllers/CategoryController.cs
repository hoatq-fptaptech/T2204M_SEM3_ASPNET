using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2204M_API.Entities;
using T2204M_API.DTOs;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace T2204M_API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly T2204mApiContext _context;

        public CategoryController(T2204mApiContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            List<CategoryDTO> list = new List<CategoryDTO>();
            foreach(var item in categories)
            {
                list.Add(new CategoryDTO {id=item.Id,name=item.Name });
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
    }
}

