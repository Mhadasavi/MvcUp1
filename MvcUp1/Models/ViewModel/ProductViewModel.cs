using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Models.ViewModel
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList{ get; set; }
    }
}
