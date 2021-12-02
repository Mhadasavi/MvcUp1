using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MvcUp1_Data;
using MvcUp1_Data.Repository.IRepository;
using MvcUp1_Model;
using MvcUp1_Model.ViewModel;
using MvcUp1_Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MvcUp1.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;
        private readonly IWebHostEnvironment _webhostenvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserVM ProductUserVm { get; set; }

        public CartController(IWebHostEnvironment webHostEnvironment, IEmailSender emailSender, IProductRepository productRepository, IApplicationUserRepository applicationUserRepository, IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailRepository inquiryDetailRepository)
        {
            _productRepository = productRepository;
            _applicationUserRepository = applicationUserRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _webhostenvironment = webHostEnvironment;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            List<int> productCartList = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodListTemp = _productRepository.GetAll(u => productCartList.Contains(u.Id));
            IList<Product> prodList = new List<Product>();
            foreach(var cartObj in shoppingCartList)
            {
                Product product = prodListTemp.FirstOrDefault(u => u.Id == cartObj.ProductId);
                product.TempGB = cartObj.TempGB;
                prodList.Add(product);
            }

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId=User.FindFirstValue(ClaimTypes.Name); 
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            List<int> productCartList = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = _productRepository.GetAll(u => productCartList.Contains(u.Id));
            ProductUserVm = new ProductUserVM()
            {
                ApplicationUser = _applicationUserRepository.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = prodList.ToList()
            };

            return View(ProductUserVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var pathToTemplate = _webhostenvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "template"
                + Path.DirectorySeparatorChar.ToString() + "inquiry.html";
            string subject = "New Enquiry";
            string htmlbody = "";
            using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
            {
                htmlbody = sr.ReadToEnd();
            }
            StringBuilder productListBuilder = new StringBuilder();
            foreach (var obj in ProductUserVm.ProductList)
            {
                productListBuilder.Append($" Name : {obj.Name} <span style='font-size:14px;'> (ID; {obj.Id})</span>");
            }
            string messageBody = string.Format(htmlbody,
                ProductUserVm.ApplicationUser.FullName,
                ProductUserVm.ApplicationUser.Email,
                ProductUserVm.ApplicationUser.PhoneNumber,
                productListBuilder.ToString()
                );

            await _emailSender.SendEmailAsync("mhadasavi@gmail.com", subject, messageBody);
            InquiryHeader inquiryHeader = new InquiryHeader()
            {
                ApplicationUserId = claim.Value,
                FullName = ProductUserVm.ApplicationUser.FullName,
                InquiryDate = DateTime.Now,
                PhoneNumber = ProductUserVm.ApplicationUser.PhoneNumber
            };

            _inquiryHeaderRepository.Add(inquiryHeader);
            _inquiryHeaderRepository.Save();

            foreach (var obj in ProductUserVm.ProductList)
            {
                InquiryDetail inquiryDetail = new InquiryDetail()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = obj.Id
                };
                _inquiryDetailRepository.Add(inquiryDetail);
            }
            _inquiryDetailRepository.Save();
           
            return RedirectToAction(nameof(InquiryConfirmation));

        }
        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
