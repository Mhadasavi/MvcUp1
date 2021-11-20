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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == WC.CategoryName)
            {
                return _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            else if (obj == WC.ApplicationName)
            {
                return _db.Application.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;

        }

        public void Update(Product product)
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
            _db.Product.Update(product);
        }
    }
}
