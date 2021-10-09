using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1_Model
{
    public class Application
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage ="Name should not be empty")]
        public String Name { get; set; }
    }
}
