namespace Muse3.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using Muse3.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Antlr.Runtime.Misc;
    using DataObjects;
    using System.Web;
    using System.IO;

    internal sealed class Configuration : DbMigrationsConfiguration<Muse3.Models.ApplicationDbContext>
    {
        private string defaultAccountImg = AppDomain.CurrentDomain.BaseDirectory + "\\MuseConfig\\ProfileImages\\defaultAccount.png";
        private string accountImageLocation = AppDomain.CurrentDomain.BaseDirectory + "\\MuseConfig\\ProfileImages\\";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Muse3.Models.ApplicationDbContext";
        }

        protected override void Seed(Muse3.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            List<string> roles = new List<string>() { "Admin", "Manager", "User", "Subscriber"};

            foreach (var roleName in roles)
            {
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = roleName });
            }

            context.SaveChanges();

            // create an admin

            string admin = "admin@company.com";
            string adminPassword = "P@ssw0rd";

            if (!context.Users.Any(u => u.UserName.Equals(admin)))
            {
                var _um = new LogicLayer.UserManager();
                var largestUserID = _um.SelectAllUsers().Select(x => x.UserID).Max();

                var user = new ApplicationUser()
                {
                    UserID = largestUserID + 1,
                    ProfileName = "System Admin",
                    UserName = admin,
                    Email = admin,
                    ImageFilePath = "defaultAccount.png",
                    FamilyName = "Admin",
                    GivenName = "System",
                    Active = true,
                    MinutesListened = 0,
                    isPublic = false
                };

                string imagePath = Path.Combine(accountImageLocation, user.ImageFilePath);

                if (File.Exists(imagePath))
                {
                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                    user.Photo = imageBytes;
                    user.PhotoMimeType = "image/png";
                }

                IdentityResult result = userManager.Create(user, adminPassword);
                context.SaveChanges();

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    userManager.AddToRole(user.Id, "Manager");
                    context.SaveChanges();
                }
            }
        }
    }
}
