using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Domain.User
{
    public class UserDataResult
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; } 
    }
}
