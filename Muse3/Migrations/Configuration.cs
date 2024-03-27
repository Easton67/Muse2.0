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

    internal sealed class Configuration : DbMigrationsConfiguration<Muse3.Models.ApplicationDbContext>
    {
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
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin,
                };

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
