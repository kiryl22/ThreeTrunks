using System;
using System.Data.Entity;
using System.Linq;
using ThreeTrunks.Data.Models;
using ThreeTrunks.Data.Repositories;

namespace ThreeTrunks.Data
{
    public static class SecurityProvider
    {
        public static User GetUser(string username)
        {
            UnitOfWork uow = new UnitOfWork();
            return uow.UserRepository.Get(u => u.Username == username).SingleOrDefault();
        }

        public static User GetCurrentUser()
        {
            return GetUser(CurrentUserName);
        }

        public static void CreateUser(User user)
        {
            User dbUser = GetUser(user.Username);
            if (dbUser != null)
                throw new Exception("User with that username already exists.");
            UnitOfWork uow = new UnitOfWork();
            uow.UserRepository.Insert(user);
            uow.Save();
        }

        public static void Register()
        {
            Database.SetInitializer(new ThreeTrunksInitializer());
            var context = new ThreeTrunksContext();
            context.Database.Initialize(true);
            if (!WebMatrix.WebData.WebSecurity.Initialized)
                WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection("ThreeTrunksContext",
                    "Users", "Id", "Username", autoCreateTables: true);
        }

        public static bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebMatrix.WebData.WebSecurity.Login(userName, password, persistCookie);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return WebMatrix.WebData.WebSecurity.ChangePassword(userName, oldPassword, newPassword);
        }

        public static bool ConfirmAccount(string accountConfirmationToken)
        {
            return WebMatrix.WebData.WebSecurity.ConfirmAccount(accountConfirmationToken);
        }

        public static void CreateAccount(string userName, string password, bool requireConfirmationToken = false)
        {
            WebMatrix.WebData.WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
        }

        public static string CreateUserAndAccount(string userName, string password, string email, bool requireConfirmationToken = false)
        {
            return WebMatrix.WebData.WebSecurity.CreateUserAndAccount(userName, password, new { Email = email }, requireConfirmationToken);
        }

        public static int GetUserId(string userName)
        {
            return WebMatrix.WebData.WebSecurity.GetUserId(userName);
        }

        public static void Logout()
        {
            WebMatrix.WebData.WebSecurity.Logout();
        }

        public static bool IsAuthenticated { get { return WebMatrix.WebData.WebSecurity.IsAuthenticated; } }

        public static string CurrentUserName { get { return WebMatrix.WebData.WebSecurity.CurrentUserName; } }
    }
}
