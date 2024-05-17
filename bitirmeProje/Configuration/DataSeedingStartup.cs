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
            if (!context.Country.Any())
            {
                await SeedCountry(context);
            }
            if (!context.City.Any())
            {
                await SeedCity(context);
            }
            if (!context.District.Any())
            {
                await SeedDistrict(context);
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
                    ImageUrl="asd",
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
                    Age=21,
                    Birthday=Convert.ToDateTime("12.07.2002"),
                    DistrictId=Guid.Parse("34cf1f6d-fd5a-48e0-9997-221182e12f2a"),
                    GenderId=Guid.Parse("d6d23f37-cda6-47f3-9bef-c9722fb2d2ce"),
                    ImageUrl="asd",
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
                    Age=22,
                    Birthday=Convert.ToDateTime("11.11.2001"),
                    DistrictId=Guid.Parse("a55adf8e-588d-44ef-abc0-57b80396f2ce"),
                    GenderId=Guid.Parse("2585ef28-f9e9-4ec0-82f5-bf04aea46a8c"),
                    ImageUrl="asd",
                    IsActive=true,
                    IsAdmin=false,
                    Mail="osman@gmail.com",
                    Password="111",
                    Username="osman",
                    PhoneNumber="55588877722",
                },

            });
        }
        public async static Task SeedDistrict(Context context)
        {
            await context.District.AddRangeAsync(new List<District>()
            {
                new District()
                {
                    Id=Guid.Parse("a55adf8e-588d-44ef-abc0-57b80396f2ce"),
                    CityId=Guid.Parse("2e08dd2b-39b6-45a7-89bb-13fea2c9c061"),
                    DistrictName="Kartal"

                },
                new District()
                {
                    Id=Guid.Parse("34cf1f6d-fd5a-48e0-9997-221182e12f2a"),
                    CityId=Guid.Parse("2e08dd2b-39b6-45a7-89bb-13fea2c9c061"),
                    DistrictName="Pendik"

                },
                new District()
                {
                    Id=Guid.Parse("b63b06d0-ad1e-4962-bc4f-436eec014973"),
                  CityId=Guid.Parse("bc8c4ef1-2f84-4dd7-9643-3c7428d6d80d"),
                  DistrictName="Çankaya"
                },
                new District()
                {
                    Id=Guid.Parse("fe743d59-368c-49c1-bb4d-da0bd56a9718"),
                  CityId=Guid.Parse("817640ae-7afc-45bc-949a-2dbdfa49c261"),
                  DistrictName="Mitte"

                },
                new District()
                {
                    Id=Guid.Parse("f27801e6-ca84-4d22-b523-60cc379d7df6"),
                    CityId=Guid.Parse("bc8c4ef1-2f84-4dd7-9643-3c7428d6d80d"),
                    DistrictName="Mamak"
                },
            });

        }
        public async static Task SeedCity(Context context)
        {
            await context.City.AddRangeAsync(new List<City>()
            {
                new City()
                {
                    Id=Guid.Parse("2e08dd2b-39b6-45a7-89bb-13fea2c9c061"),
                    CountryId=Guid.Parse("0a6770f0-d446-42de-bf07-0b411b0fb298"),
                    CityName="İstanbul"
                },
                new City()
                {
                    Id=Guid.Parse("bc8c4ef1-2f84-4dd7-9643-3c7428d6d80d"),
                    CountryId=Guid.Parse("0a6770f0-d446-42de-bf07-0b411b0fb298"),
                    CityName="Ankara"
                },
                new City()
                {
                    Id=Guid.Parse("817640ae-7afc-45bc-949a-2dbdfa49c261"),
                    CountryId=Guid.Parse("4ce5bc87-329c-4b55-a59d-c9d226c13a38"),
                    CityName="Berlin"
                },
            });
        }
        public async static Task SeedCountry(Context context)
        {
            await context.Country.AddRangeAsync(new List<Country>()
            {

                new Country
                {
                    Id=Guid.Parse("0a6770f0-d446-42de-bf07-0b411b0fb298"),
                    CountryName="Türkiye"
                },
                 new Country
                {
                    Id=Guid.Parse("4ce5bc87-329c-4b55-a59d-c9d226c13a38"),
                    CountryName="Almanya"
                }
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
