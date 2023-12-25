using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    // this class will hold extention helpers
    // extension methods must be in a public class
    public static class ValidationHelpers
    {
        // extension methods must be public, static, and include'this' 
        // as the first parameter, with the type being extended following

        public static bool IsValidEmail(this string email)
        {
            bool isValid = false;

            if (email.Contains("@") && 
                email.Contains(".") && 
                email.Length >= 10 && 
                email.Length <= 100)
            {
                isValid = true;
            }
            return isValid;
        }
        public static bool IsValidProfileName(this string profileName)
        {
            bool isValid = false;
            if (profileName.Length >= 1 &&
                profileName.Length <= 100)
            {
                isValid = true;
            }
            return isValid;
        }
        public static bool IsValidPassword(this string password)
        {
            bool isValid = false;

            if (password.Length >= 7)
            {
                isValid = true;
            }

            return isValid;
        }
        public static bool IsValidFirstName(this string FirstName)
        {
            bool isValid = false;

            // what the db has as its max
            if (FirstName.Length <= 50 && FirstName.Length > 0)
            {
                isValid = true;
            }

            return isValid;
        }
        public static bool IsValidLastName(this string LastName)
        {
            bool isValid = false;

            // what the db has as its max
            if (LastName.Length <= 50 && LastName.Length > 0)
            {
                isValid = true;
            }
            return isValid;
        }
        public static bool IsValidMP3(this string MP3FilePath)
        {
            bool isValid = false;

            // what the db has as its max
            if (MP3FilePath.Length <= 500 && 
                MP3FilePath.Length > 0 &&
                MP3FilePath.Contains(".mp3"))
            {
                isValid = true;
            }
            return isValid;
        }
        public static bool IsDefaultImage(this string ImageFilePath)
        {
            bool isDefault = false;

            if (ImageFilePath.Length == 0)
            {
                isDefault = true;
            }
            return isDefault;
        }
        public static bool IsValidYear(this int Year)
        {
            bool isValid = false;

            if (Year <= 2023 && 
                Year >= 1700)
            {
                isValid = true;
            }
            return isValid;
        }
    }
}
