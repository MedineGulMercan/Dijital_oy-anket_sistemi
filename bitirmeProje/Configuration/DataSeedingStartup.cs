using bitirmeProje.Domain.DataBaseContext;
using bitirmeProje.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace bitirmeProje.Configuration
{
    public static class DataSeedingStartup
    {
        public async static Task Seed(IApplicationBuilder app)
        {
            //inject yapmak için scop oluşturuyoruz
            var scope = app.ApplicationServices.CreateScope();
            //context nesnesini scope ile çekiyoruz
            var context = scope.ServiceProvider.GetService<Context>();

            //migration'ları sistem her çalıştığımda çalıştırır (NOT: sadece çalışmamışları)
            //çalışmadığını ise db içerisinde otomatik oluşan __EFMigrationsHistory tablosunda kayıtlı değilse migration çalışmamış demektir

            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                throw;
            }
            if (!context.Gender.Any())
            {
                await SeedGender(context);
            }
            if (!context.Users.Any())
            {
                await SeedUser(context);
            }
            if (!context.Group.Any())
            {
                await SeedGroup(context);
            }
            if (!context.Roles.Any())
            {
                await SeedRoles(context);
            }
            if (!context.GroupUser.Any())
            {
                await SeedGroupUser(context);
            }
            
            await context.SaveChangesAsync();

        }
        public async static Task SeedGroupUser(Context context)
        {
            await context.GroupUser.AddRangeAsync(new List<GroupUser>()
            {
                new GroupUser()
                {
                    Id= Guid.Parse("bbbe1641-a1de-47da-bacc-82f75859d3ee"),
                    UserId=Guid.Parse("7fb4fb3a-0751-45c6-9beb-793ea538db94"),
                    GroupId=Guid.Parse("f90af7b3-0a0a-485d-b4c4-d73bc40d47e7"),
                    IsActive=true,
                    IsMember=true,
                    RoleId=Guid.Parse("4fe122de-0fee-4727-b7d0-5a76f8fb0e45")
                },
                new GroupUser()
                {
                    Id= Guid.Parse("1f92ac6f-0630-4fe0-bcd0-ba8bec94e7d8"),
                    UserId=Guid.Parse("2cfaef38-2fd7-4bb0-9812-f33ad8dd0a08"),
                    GroupId=Guid.Parse("f90af7b3-0a0a-485d-b4c4-d73bc40d47e7"),
                    IsActive=true,
                    IsMember=true,
                    RoleId=Guid.Parse("6394dba5-1fc7-4a3b-8084-0cf1b213a225")
                }
            });
        }

        public async static Task SeedRoles(Context context)
        {
            await context.Roles.AddRangeAsync(new List<Role>()
            {
                new Role()
                {
                    Id=Guid.Parse("4fe122de-0fee-4727-b7d0-5a76f8fb0e45"),
                    RoleName="Yönetici",
                },
                new Role()
                {
                    Id=Guid.Parse("6394dba5-1fc7-4a3b-8084-0cf1b213a225"),
                    RoleName="Üye",
                }
            });
        }
        public async static Task SeedGroup(Context context)
        {
            await context.Group.AddRangeAsync(new List<Group>()
            {
                new Group()
                {
                    Id=Guid.Parse("f90af7b3-0a0a-485d-b4c4-d73bc40d47e7"),
                    GroupName="Public",
                    GroupDescription="Herkes üye",
                    CanCreateSurvey=true,
                    GroupOwnerId=Guid.Parse("7fb4fb3a-0751-45c6-9beb-793ea538db94"),
                    ImageUrl="",
                    IsActive=true,
                }
            });
        }
        public async static Task SeedUser(Context context)
        {
            await context.Users.AddRangeAsync(new List<User>()
            {
                  new User()
                {
                    Id=Guid.Parse("7fb4fb3a-0751-45c6-9beb-793ea538db94"),
                    Name="Medine Gül",
                    Surname="Mercan",
                    Birthday=Convert.ToDateTime("12.07.2002"),
                    GenderId=Guid.Parse("d6d23f37-cda6-47f3-9bef-c9722fb2d2ce"),
                    ImageUrl="",
                    IsActive=true,
                    IsAdmin=true,
                    Mail="med@gmail.com",
                    Password="222",
                    Username="medine",
                    PhoneNumber="55588877733",
                },
                new User()
                {
                    Id=Guid.Parse("2cfaef38-2fd7-4bb0-9812-f33ad8dd0a08"),
                    Name="Osman",
                    Surname="Sivrikaya",
                    Birthday=Convert.ToDateTime("11.11.2001"),
                    GenderId=Guid.Parse("2585ef28-f9e9-4ec0-82f5-bf04aea46a8c"),
                    ImageUrl="",
                    IsActive=true,
                    IsAdmin=false,
                    Mail="osman@gmail.com",
                    Password="111",
                    Username="osman",
                    PhoneNumber="55588877722",
                },

            });
        }
        public async static Task SeedGender(Context context)
        {
            await context.Gender.AddRangeAsync(new List<Gender>()
            {
                new Gender
                {
                  Id=Guid.Parse("2585ef28-f9e9-4ec0-82f5-bf04aea46a8c"),
                  GenderName="Erkek"
                },
                 new Gender
                 {
                  Id=Guid.Parse("d6d23f37-cda6-47f3-9bef-c9722fb2d2ce"),
                  GenderName="Kız"
                 }

            });
        }

    }
}
