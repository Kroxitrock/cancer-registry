using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts
{
    public class ChangePassword
    {
        [Required, DataType(DataType.Password)]
        public String Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public String ConfirmPassword { get; set; }

        public String AccountId { get; set; }
    }
}
