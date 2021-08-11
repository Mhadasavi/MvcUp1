using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
