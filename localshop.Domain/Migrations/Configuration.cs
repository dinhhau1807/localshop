namespace localshop.Domain.Migrations
{
    using localshop.Domain.Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<localshop.Domain.Concretes.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(localshop.Domain.Concretes.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

            string roleName = "root";
            string password = "Admin123@localshop.hau";
            string email = "admin@example.com";

            string[] seedRoles = { "administrator", "modifier", "customer" };
            foreach (var role in seedRoles)
            {
                if (!roleManager.RoleExists(role))
                {
                    roleManager.Create(new ApplicationRole(role));
                }
            }

            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new ApplicationRole(roleName));
            }

            var userTemp = userManager.FindByName(email);
            if (userTemp == null)
            {
                userManager.Create(new ApplicationUser
                {
                    FirstName = "Hau",
                    LastName = "Nguyen Dinh",
                    UserName = email,
                    Email = email
                }, password);

                userTemp = userManager.FindByName(email);
            }

            if (!userManager.IsInRole(userTemp.Id, roleName))
            {
                userManager.AddToRole(userTemp.Id, roleName);
            }

            context.SaveChanges();
        }
    }
}
