using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Services
{
    public class OperationResult
    {
        public bool Succeeded { get; set; } = true;

        public List<string> Errors { get; set; }
    }
}
