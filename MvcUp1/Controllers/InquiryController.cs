using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcUp1_Data.Repository.IRepository;
using MvcUp1_Model;
using MvcUp1_Model.ViewModel;
using MvcUp1_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Controllers
{
    [Authorize(Roles=WC.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;
        [BindProperty]
        public InquiryVm InquiryVm { get; set; }
        public InquiryController(IInquiryDetailRepository inquiryDetailRepository, IInquiryHeaderRepository inquiryHeaderRepository)
        {
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            InquiryVm = new InquiryVm()
            {
                InquiryHeader = _inquiryHeaderRepository.FirstOrDefault(u => u.Id == id),
                InquiryDetails = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == id, includeProperties: "Product")
            };

            return View(InquiryVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            InquiryVm.InquiryDetails = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == InquiryVm.InquiryHeader.Id);
            foreach (var detail in InquiryVm.InquiryDetails)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detail.Id
                };
                shoppingCarts.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVm.InquiryHeader.Id);
            return RedirectToAction("Index", "Cart");
        }
        #region API Call
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _inquiryHeaderRepository.GetAll() });
        }
        #endregion

        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _inquiryHeaderRepository.FirstOrDefault(u => u.Id == InquiryVm.InquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == InquiryVm.InquiryHeader.Id);

            _inquiryHeaderRepository.Remove(inquiryHeader);
            _inquiryDetailRepository.RemoveRange(inquiryDetails);
            _inquiryDetailRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
