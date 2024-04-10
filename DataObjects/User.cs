using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class User
    {
        [DisplayName("UserID")]
        public int UserID { get; set; }
        [DisplayName("Profile Name")]
        public string ProfileName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Image File Path")]
        public string ImageFilePath { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoMimeType { get; set; }
        [DisplayName("Active User")]
        public bool Active { get; set; }    
        [DisplayName("Minutes Listened")]
        public int MinutesListened { get; set; }
        [DisplayName("Public")]
        public bool isPublic { get; set; }
    }

    public class UserVM : User
    {
        public List<string> Roles { get; set; }
    }
    public class UserPass : User
    {
        public string PasswordHash { get; set; }
    }
    public class UserFriend : User
    {
        public DateTime DateAddedAsFriend { get; set; }
    }
}
