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

            users.Add(new User() { UserName = "bondarets.bogdan@gmail.com", FirstName = "Bohdan", LastName = "Bondarets", Email = "bondarets.bogdan@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "yuravasko2016@gmail.com", FirstName = "Yura", LastName = "Vasko", Email = "yuravasko2016@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "yurikozlovskiJ@gmail.com", FirstName = "Yura", LastName = "Kozlovsky", Email = "yurikozlovskiJ@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "nazarp06@gmail.com", FirstName = "Nazar", LastName = "Polevyy", Email = "nazarp06@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "kravchenkov.me@gmail.com", FirstName = "Valentyn", LastName = "Kravchenko", Email = "kravchenkov.me@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "yura.stashko98@gmail.com", FirstName = "Yura", LastName = "Stashko", Email = "yura.stashko98@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "solayusko@gmail.com", FirstName = "Solomia", LastName = "Yusko", Email = "solayusko@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "flyssofia@gmail.com", FirstName = "Sofia", LastName = "Flys", Email = "flyssofia@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });

            users.Add(new User() { UserName = "admino@gmail.com", FirstName = "El", LastName = "Admino", Email = "admino@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 3 });
            users.Add(new User() { UserName = "admina@gmail.com", FirstName = "La", LastName = "Admina", Email = "admina@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 3 });

            users.Add(new User() { UserName = "nosadchuk@gmail.com", FirstName = "Nikita", LastName = "Nosadchuk", Email = "nosadchuk@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
            users.Add(new User() { UserName = "kondor@gmail.com", FirstName = "Marta", LastName = "Kondor", Email = "kondor@gmail.com", Blocked = false, EmailConfirmed = true, Role_Id = 2 });
           
            foreach (var user in users)
            {
                var userExist = await userManager.FindByEmailAsync(user.Email);
                if (userExist == null)
                {
                    userResult = await userManager.CreateAsync(user, GeneralPassword);
                    var add_role = await userManager.AddToRoleAsync(user, user.Role.Name);
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
        }
    }
}