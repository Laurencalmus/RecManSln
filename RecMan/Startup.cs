using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RecMan.DAL;
using RecMan.Models;

[assembly: OwinStartupAttribute(typeof(RecMan.Startup))]
namespace RecMan
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }


        // In this method we will create default User roles and Admin user for login    
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new Microsoft.AspNet.Identity.RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // creates Admin role  
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //creates an Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "Lauren";
                user.Email = "laurencalmus@gmail.com";

                string userPWD = "Icetc0c0-";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Teacher role     
            if (!roleManager.RoleExists("Teacher"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Teacher";
                roleManager.Create(role);

            }
        }
    }
}
