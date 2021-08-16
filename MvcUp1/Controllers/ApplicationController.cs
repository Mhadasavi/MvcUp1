using Microsoft.AspNetCore.Mvc;
using MvcUp1.Data;
using MvcUp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Controllers
{
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
    }
}
