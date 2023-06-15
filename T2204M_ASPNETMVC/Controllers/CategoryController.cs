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
    }
}

