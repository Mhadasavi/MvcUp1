using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name{ get; set; }
        public String Description { get; set; }
        [Range(1,int.MaxValue)]
        public double Price { get; set; }
        public String Image { get; set; }
        [Display(Name="Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [Display(Name="Application Type")]
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }
    }
}
