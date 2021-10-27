using Microsoft.AspNetCore.Mvc.Rendering;
using MvcUp1_Data.Repository.IRepository;
using MvcUp1_Model;
using MvcUp1_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcUp1_Data.Repository
{
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public InquiryDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InquiryDetail inquiryDetail)
        {
            //var objFromDb = base.FirstOrDefault(u => u.Id == product.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Name = product.Name;
            //    objFromDb.ShortDesc = product.ShortDesc;
            //    objFromDb.Description = product.Description;
            //    objFromDb.Price = product.Price;
            //    objFromDb.Image = product.Image;
            //    objFromDb.CategoryId = product.CategoryId;
            //    objFromDb.Category = product.Category;
            //    objFromDb.Application = product.Application;
            //    objFromDb.ApplicationId = product.ApplicationId;
            //}
            _db.InquiryDetail.Update(inquiryDetail);
        }
    }
}
