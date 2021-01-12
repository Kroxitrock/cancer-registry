using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Полето \"Текуща парола\" е задължително."), DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Полето \"Нова парола\" е задължително."), DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Нужно е да потвърдите паролата."),DataType(DataType.Password), Compare(nameof(NewPassword), ErrorMessage = "Паролите не съвпадат")]
        public string ConfirmNewPassword { get; set; }

        public string AccountId { get; set; }
    }
}
