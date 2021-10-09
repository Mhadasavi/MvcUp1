using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcUp1.Data;
using MvcUp1_Model;
using MvcUp1_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _db.Category;
            return View(categoryList);
        }
        //httpget
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        //get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _db.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = id;
                _db.Category.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        //get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _db.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute]int id)
        {
            var Category = _db.Category.Find(id);
            if (Category == null)
            {
                return NotFound();
            }
            _db.Category.Remove(Category);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
