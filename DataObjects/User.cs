using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class User
    {
        public int UserID { get; set; }
        public string ProfileName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageFilePath { get; set; }
        public bool Active { get; set; }
        public bool Private { get; set; }
    }

    public class UserVM : User
    {
        public List<string> Roles { get; set; }
    }
}
