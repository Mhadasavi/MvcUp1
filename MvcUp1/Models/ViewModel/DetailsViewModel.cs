using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Models.ViewModel
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Product = new Product();
        }
        public Product Product { get; set; }
        public bool IsExist { get; set; }
    }
}
