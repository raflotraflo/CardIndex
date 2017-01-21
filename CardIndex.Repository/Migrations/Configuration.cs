namespace CardIndex.Repository.Migrations
{
    using Domain.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CardIndex.Repository.RepositoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CardIndex.Repository.RepositoryContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
            {

                System.Diagnostics.Debugger.Launch();

            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            SeedContractors(context);
        }

        private void SeedContractors(RepositoryContext context)
        {
            try
            {
                var dbsetContractor = context.Set<Contractor>();
                var dbsetAddress = context.Set<Address>();

                for (int i = 1; i <= 5; i++)
                {
                    var newContractor = new Contractor() {
                        Name = "Rafa³ " + i,
                        Surname = "Chodzid³o",
                        Email = "raflotraflo@gmail.com",
                        PhoneNumber = "530529985",
                        DateOfBirth = new DateTime(1992, 3, 17)
                    };

                    if (i % 2 == 1)
                    {
                        var newAdress = new Address()
                        {
                            Street = "Wieczorka " + i,
                            City = "Studzionka",
                            ZipCode = "43-245"
                        };

                        newContractor.Address = dbsetAddress.Add(newAdress);
                    }

                    dbsetContractor.AddOrUpdate(newContractor);

                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                ;
            }
        }

        //private void SeedAddress(RepositoryContext context)
        //{
        //    var dbsetAddress = context.Set<Address>();

        //            var newAdress = new Address()
        //            {
        //                Street = "Wieczorka " + i,
        //                City = "Studzionka",
        //                ZipCode = "43-245"
        //            };

        //    dbsetAddress.AddOrUpdate(newAdress);


        //    context.SaveChanges();
        //}
    }
}
