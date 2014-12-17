using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;

namespace ThreeTrunks.Data
{
    static public class DataInitializer
    {
        public static void Initilaize()
        {
            Database.SetInitializer<ThreeTrunksContext>(new ThreeTrunksInitializer());
            using (var context = new ThreeTrunksContext())
            {
                context.Database.Initialize(true);

                if (!WebSecurity.Initialized)
                {
                    WebSecurity.InitializeDatabaseConnection("ThreeTrunksContext", "Users", "Id", "Username", autoCreateTables: true);

                    var roles = (SimpleRoleProvider)Roles.Provider;
                    var membership = (SimpleMembershipProvider)Membership.Provider;

                    if (!roles.RoleExists("Admin"))
                    {
                        roles.CreateRole("Admin");
                    }
                    if (membership.GetUser("admin", false) == null)
                    {
                        membership.CreateUserAndAccount("admin", "1234");
                    }
                    if (!roles.GetRolesForUser("admin").Contains("Admin"))
                    {
                        roles.AddUsersToRoles(new[] { "admin" }, new[] { "Admin" });
                    }
                }
            }
        }
    }
}
