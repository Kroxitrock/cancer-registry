using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String EGN { get; set; }

        public String UID { get; set; }
    }
}
