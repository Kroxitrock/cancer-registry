using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts
{
    public class PasswordReset
    {
        [Required(ErrorMessage = "Полето \"Парола\" е задължително."), DataType(DataType.Password)]
        public String Password { get; set; }
        
        [Required(ErrorMessage = "Нужно е да потвърдите паролата."), DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Паролите не съвпадат.")]
        public String ConfirmPassword { get; set; }
        
        public String Username { get; set; }
        
        public String Token { get; set; }
    }
}
