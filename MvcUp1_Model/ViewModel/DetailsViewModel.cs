using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1_Model.ViewModel
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Product = new Product();
        }
        public Product Product { get; set; }
        public bool ExistInCart { get; set; }
    }
}
