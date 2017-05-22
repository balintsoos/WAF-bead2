using Service.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Service.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AdministratorDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AdministratorDbContext context)
        {
            // adatbázis feltöltése adatokkal

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                // ammenyiben még nincs admin felhasználó
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                UserManager<IdentityGuest> userManager = new UserManager<IdentityGuest>(new UserStore<IdentityGuest>(context));

                IdentityRole role = new IdentityRole { Name = "administrator" }; // létrehozzuk az administrator csoportot
                roleManager.Create(role);

                IdentityGuest adminUser = new IdentityGuest { UserName = "admin" }; // létrehozzuk az admin felhasználót a csoportban
                userManager.Create(adminUser, "password");
                userManager.AddToRole(adminUser.Id, "administrator");
            }
        }
    }
}