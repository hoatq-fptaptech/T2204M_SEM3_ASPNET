using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2204M_ASPNETMVC.Entities;
using Microsoft.EntityFrameworkCore;
using T2204M_ASPNETMVC.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace T2204M_ASPNETMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _context;

        public CategoryController(DataContext dataContext)
        {
            _context = dataContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories
                //.Where(c => c.Name.Contains("ash"))
                //.OrderBy(c=>c.Name)
                //.OrderByDescending(c=>c.Name)
                .Include(c=>c.Products)
                //.Take(1)
                //.Skip(1)
                .ToList<Category>();
            //ViewData["categories"] = categories;
            //ViewBag.Categories = categories;
            return View(categories);
        }

        public IActionResult Create()
        {
            //ViewBag.categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(new Category { Name=viewModel.Name });
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var category = _context.Categories.Find(Id);
            if (category == null)
                return NotFound();
            return View(new EditCategoryViewModel { Id=Id,Name=category.Name});
        }

        [HttpPost]
        public IActionResult Edit(EditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(new Category { Id = model.Id, Name = model.Name });
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var category = _context.Categories.Find(Id);
            if(category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile Image)
        {
            if(Image == null)
            {
                return BadRequest("Vui lòng up file đính kèm");
            }
            var path = "wwwroot/uploads";
            var fileName = Guid.NewGuid().ToString() + Path.GetFileName(Image.FileName);
            var upload = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
            Image.CopyTo(new FileStream(upload, FileMode.Create));
            var rs = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
            return Ok(rs);
        }
    }
}

