using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Admin
{
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "Field \"Username\" is required.")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Password is required."), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
