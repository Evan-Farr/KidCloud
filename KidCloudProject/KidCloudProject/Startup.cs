using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using KidCloudProject.Models;

[assembly: OwinStartupAttribute(typeof(KidCloudProject.Startup))]
namespace KidCloudProject
{
    public partial class Startup
    {
        DateTime currentDate = DateTime.Now;
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
            app.MapSignalR();
            CreateInvoice();
        }

        private void CreateRolesAndUsers()
        {
            Models.ApplicationDbContext context = new Models.ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<Models.ApplicationUser>(new UserStore<Models.ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                var user = new Models.ApplicationUser();
                user.UserName = "Admin";
                user.Email = "admin@gmail.com";
                string userPassword = "Admin123!";
                var checkUser = userManager.Create(user, userPassword);
                if (checkUser.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Parent"))
            {
                var role = new IdentityRole();
                role.Name = "Parent";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("DayCare"))
            {
                var role = new IdentityRole();
                role.Name = "DayCare";
                roleManager.Create(role);
            }
        }

        public void CreateInvoice()
        {
            double halfDayCharge;
            decimal childTotal;

            if (currentDate.Day == 12)
            {
                var invoiceAccounts = db.DayCares.Select(d => d.Parents).ToList();

                foreach (var listOfParents in invoiceAccounts)
                {
                    foreach (var parent in listOfParents)
                    {
                        if (parent.DayCare != null)
                        {
                            if (parent.MoneyOwed == null)
                            {
                                parent.MoneyOwed = 0;
                            }
                            var dayCareCharge = parent.DayCare.DailyRatePerChild;
                            var listOfChildren = parent.Children;
                            foreach (var child in listOfChildren)
                            {
                                if (child.HalfDay == true)
                                {
                                    halfDayCharge = dayCareCharge / 2;
                                    childTotal = System.Convert.ToDecimal(halfDayCharge * 22);
                                    parent.MoneyOwed += childTotal;
                                    break;     
                                }
                                else if(child.FullDay == true)
                                {
                                    childTotal = System.Convert.ToDecimal(dayCareCharge * 22);
                                    parent.MoneyOwed += childTotal;
                                    break;
                                }
                                    
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
        }


    }
}
