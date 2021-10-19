using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcUp1_Data;
using MvcUp1_Data.Repository.IRepository;
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
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationController(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Application> application = _applicationRepository.GetAll();
            return View(application);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Application application)
        {
            _applicationRepository.Add(application);
            _applicationRepository.Save();
            return RedirectToAction("Index");
        }//get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Application = _applicationRepository.Find(id.GetValueOrDefault());
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
                _applicationRepository.Update(application);
                _applicationRepository.Save();
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
            var Application = _applicationRepository.Find(id.GetValueOrDefault());
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
            var application = _applicationRepository.Find(id);
            if (application==null)
            {
                return NotFound();
            }
            _applicationRepository.Remove(application);
            _applicationRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
