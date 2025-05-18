using ATON_Test_Ploblem.Models;
using System.Collections.Generic;

namespace ATON_Test_Ploblem.Services
{
    public class UserRepository
    {
        private List<User> Users = new List<User>();

        public UserRepository()
        {
            var Admin = new User
            {
                Guid = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                Login = "Admin",
                Password = "password",
                Name = "Admin",
                Gender = 0,
                Birthday = new DateTime(2004, 10, 18),
                Admin = true,
                CreatedOn = DateTime.Now,
                CreatedBy = "Admin",
                ModifiedOn = DateTime.Now,
                ModifiedBy = "Admin",
            };

            Add(Admin);
        }

        public User? GetActiveByLogin(string login)
        {
            return Users.SingleOrDefault(x => x.Login == login && x.RevokedOn is null);
        }

        public User? GetByLogin(string login)
        {
            return Users.SingleOrDefault(x => x.Login == login);
        }

        public User? GetById(Guid guid)
        {
            return Users.SingleOrDefault(x => x.Guid == guid);
        }

        public void Add(User user)
        {
            Users.Add(user);
        }

        public void Update(User user)
        {
            var oldUser = GetActiveByLogin(user.Login);

            if (oldUser is not null)
            {
                var indexOfOldUser = Users.IndexOf(oldUser);
                Users[indexOfOldUser] = user;
            }
        }

        public List<User> GetAllActive()
        {
            var activeUsers = Users.Where(u => u.RevokedOn is null).ToList();

            return activeUsers;
        }

        public List<User> GetOlderThen(int age)
        {
            return Users.Where(u => u.Birthday is not null && DateTime.Now.Year - u.Birthday.Value.Year > age).ToList();
        }
    }
} 
