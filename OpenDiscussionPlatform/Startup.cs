using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using OpenDiscussionPlatform.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenDiscussionPlatform.Startup))]
namespace OpenDiscussionPlatform
{

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Se apeleaza o metoda in care se adauga contul de administrator si rolurile aplicatiei
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            
            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Admin"))  
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "mateidinescu@gmail.com";
                user.Email = "mateidinescu@gmail.com";
               // user.Birthdate = new System.DateTime(2015, 12, 25);
                var adminCreated = UserManager.Create(user, "!1Admin");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }

                var user2 = new ApplicationUser();
                user2.UserName = "madalinfrincu@gmail.com";
                user2.Email = "madalinfrincu@gmail.com";
                var adminCreated2 = UserManager.Create(user2, "!1Admin");
                if (adminCreated2.Succeeded)
                {
                    UserManager.AddToRole(user2.Id, "Admin");
                }

                var user3 = new ApplicationUser();
                user3.UserName = "admin@gmail.com";
                user3.Email = "admin@gmail.com";
                var adminCreated3 = UserManager.Create(user3, "!1Admin");
                if (adminCreated3.Succeeded)
                {
                    UserManager.AddToRole(user3.Id, "Admin");
                }


            }
            if (!roleManager.RoleExists("E-ditor"))
            {
                var role = new IdentityRole();
                role.Name = "E-ditor";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }

    }
}

