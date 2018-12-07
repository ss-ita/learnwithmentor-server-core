namespace LearnWithMentor.DAL.Entities
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using LearnWithMentor.DAL.Entities;

    public static class IdentityDataInitializer
    {
        public static async Task SeedData(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        public static async Task SeedUsers(UserManager<User> userManager)
        {
            IdentityResult userResult;
            const string GeneralPassword = "123";
            List<User> users = new List<User>();
            users.Add(new User() { UserName = "koldovsky@gmail.com", FirstName = "Vyacheslav", LastName = "Koldovsky", Email = "koldovsky@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "romaniv@gmail.com", FirstName = "Khrystyna", LastName = "Romaniv", Email = "romaniv@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "khoroshchak@gmail.com", FirstName = "Orysia", LastName = "Khoroshchak", Email = "khoroshchak@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "klakovych@gmail.com", FirstName = "Lesya", LastName = "Klakovych", Email = "klakovych@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "ryazhska@gmail.com", FirstName = "Viktoria", LastName = "Ryazhska", Email = "ryazhska@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "halamaha@gmail.com",  FirstName = "Liubomyr", LastName = "Halamaha", Email = "halamaha@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "kohut@gmail.com", FirstName = "Igor", LastName = "Kohut", Email = "kohut@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "korkuna@gmail.com", FirstName = "Andriy", LastName = "Korkuna", Email = "korkuna@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "harasym@gmail.com", FirstName = "Yaroslav", LastName = "Harasym", Email = "harasym@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "plesna@gmail.com", FirstName = "Mykhaylo", LastName = "Plesna", Email = "plesna@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });
            users.Add(new User() { UserName = "lopatynska@gmail.com", FirstName = "Maryana", LastName = "Lopatynska", Email = "lopatynska@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 1 });

            users.Add(new User() { UserName = "nosadchuk@gmail.com", FirstName = "Nikita", LastName = "Nosadchuk", Email = "nosadchuk@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "kondor@gmail.com", FirstName = "Marta", LastName = "Kondor", Email = "kondor@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
           

            foreach (var user in users)
            {
                var userExist = await userManager.FindByEmailAsync(user.Email);
                if (userExist == null)
                {
                    userResult = await userManager.CreateAsync(user, GeneralPassword);
                }
            }
        }

        public static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            string[] roleNames = { "Mentor", "Student", "Admin" };
            IdentityResult roleResult;
            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new Role(role));
                }
            }

            /*if (!roleManager.RoleExistsAsync
                ("Admin").Result)
            {
                Role role = new Role();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Mentor").Result)
            {
                Role role = new Role();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }*/
        }
    }
}