using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUp1_Model
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
