using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcUp1_Data;
using MvcUp1_Data.Repository.IRepository;
using MvcUp1_Model;
using MvcUp1_Model.ViewModel;
using MvcUp1_Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _productRepository.GetAll(includeProperties: "Category,Application");
            //foreach (var objList in productList)
            //{
            //    objList.Category = _productRepository.Category.FirstOrDefault(u => u.Id == objList.CategoryId);
            //    objList.Application = _productRepository.Application.FirstOrDefault(u => u.Id == objList.ApplicationId);
            //}
            return View(productList);
        }
        //httpget
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _productRepository.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});
            ////            ViewBag.CategoryDropDown = CategoryDropDown;
            //ViewData["CategoryDropDown"] = CategoryDropDown;

            //Product product = new Product();
            ProductViewModel productViewModel = new ProductViewModel()
            {
                product = new Product(),
                CategorySelectList = _productRepository.GetAllDropDownList(WC.CategoryName),
                ApplicationSelectList = _productRepository.GetAllDropDownList(WC.ApplicationName)
            };
            if (id == null)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.product = _productRepository.Find(id.GetValueOrDefault());
                if (productViewModel.product == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productViewModel.product.Id == 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productViewModel.product.Image = fileName + extension;
                    _productRepository.Add(productViewModel.product);

                }
                else
                {
                    var objFromDb = _productRepository.FirstOrDefault(i => i.Id == productViewModel.product.Id,isTracking:false);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productViewModel.product.Image = fileName + extension;
                    }
                    else
                    {
                        productViewModel.product.Image = objFromDb.Image;
                    }
                    _productRepository.Update(productViewModel.product);
                }
                _productRepository.Save();
                return RedirectToAction("Index");
            }
            productViewModel.CategorySelectList = _productRepository.GetAllDropDownList(WC.CategoryName);
            productViewModel.ApplicationSelectList = _productRepository.GetAllDropDownList(WC.ApplicationName);
            return View(productViewModel);
        }
        //get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _productRepository.FirstOrDefault(u=>u.Id==id,includeProperties: "Category,Application");
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            var product = _productRepository.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            var oldFile = Path.Combine(upload, product.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _productRepository.Remove(product);
            _productRepository.Save();
            return RedirectToAction("Index");

        }
    }
}
