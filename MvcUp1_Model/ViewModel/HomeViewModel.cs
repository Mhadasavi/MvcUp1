using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1_Model.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Category> Category { get; set; }
    }
}
