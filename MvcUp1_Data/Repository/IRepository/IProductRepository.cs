using Microsoft.AspNetCore.Mvc.Rendering;
using MvcUp1_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcUp1_Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
        IEnumerable<SelectListItem> GetAllDropDownList(string obj);
    }
}
