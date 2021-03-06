using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1_Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        [DisplayName("Display Order")]
        [Range(1,int.MaxValue,ErrorMessage ="Display Order must be greater than 0 ")]
        public int DisplayOrder { get; set; }
    }
}
