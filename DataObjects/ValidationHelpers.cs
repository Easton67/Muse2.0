﻿using System;
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
            if (FirstName.Length <= 50)
            {
                isValid = true;
            }

            return isValid;
        }
        public static bool IsValidLastName(this string LastName)
        {
            bool isValid = false;

            // what the db has as its max
            if (LastName.Length <= 50)
            {
                isValid = true;
            }

            return isValid;
        }
        public static bool IsValidProfileName(this string ProfileName)
        {
            bool isValid = false;

            // what the db has as its max
            if (ProfileName.Length <= 200)
            {
                isValid = true;
            }

            return isValid;
        }
    }
}
