using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class ReferralsFixture : BaseTest
    {
        private readonly ILoggerFactory loggerFactory;

        public ReferralsFixture(ITestOutputHelper output) : base(output)
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(new XUnitLoggerProvider(this));
        }

        [Fact]
        public void CanInsertNewClothingReferral()
        {
            var ctx = new EmbcDbContext(new DbContextOptionsBuilder<EmbcDbContext>()
                .UseLoggerFactory(loggerFactory)
                .UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ESS_develop;Integrated Security=True;").Options);

            var registrationID = 100000L;

            var referral = new ClothingReferral()
            {
                Comments = "comments",
                ExtremeWeatherConditions = true,
                RegistrationId = registrationID,
                ValidFrom = DateTime.Parse("2019-02-28T03:30:44"),
                ValidTo = DateTime.Parse("2019-03-04T11:00:00"),
                Supplier = new Supplier()
                {
                    Name = "supplier1",
                    Address = "address",
                    City = "vancity"
                },
                Evacuees = new[]
                {
                    new ReferralEvacuee{ RegistrationId = registrationID, EvacueeId=1, IsPurchaser=true },
                    new ReferralEvacuee{ RegistrationId = registrationID, EvacueeId=2, IsPurchaser=false }
                },
                TotalAmount = 100.23m,
                ConfirmChecked = true
            };

            ctx.Add(referral);
            ctx.SaveChanges();
        }
    }
}