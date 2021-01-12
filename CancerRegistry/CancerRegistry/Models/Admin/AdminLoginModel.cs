using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Admin
{
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "Полето \"Потребителско име\" е задължително.")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Паролата е задължителна."), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
