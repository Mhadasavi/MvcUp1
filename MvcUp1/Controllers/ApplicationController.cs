using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcUp1_Data;
using MvcUp1_Model;
using MvcUp1_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationController : Controller
    {
        private ApplicationDbContext _db;
        public ApplicationController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Application> application = _db.Application;
            return View(application);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Application application)
        {
            _db.Application.Add(application);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }//get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Application = _db.Application.Find(id);
            if (Application == null)
            {
                return NotFound();
            }
            return View(Application);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Application application)
        {
            if (ModelState.IsValid)
            {
                application.Id = id;
                _db.Application.Update(application);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(application);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Application = _db.Application.Find(id);
            if (Application == null)
            {
                return NotFound();
            }
            return View(Application);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            var application = _db.Application.Find(id);
            if (application==null)
            {
                return NotFound();
            }
            _db.Application.Remove(application);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
