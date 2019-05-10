using AutoMapper;
using Gov.Jag.Embc.Public;
using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class BaseTest
    {
        private ServiceProvider serviceProvider;

        protected IMapper mapper => serviceProvider.GetService<IMapper>();

        protected EmbcDbContext EmbcDb => serviceProvider.GetService<EmbcDbContext>();

        public BaseTest(ITestOutputHelper output)
        {
            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddProvider(new XUnitLoggerProvider(output)))
                .AddAutoMapper(typeof(Startup))
                .AddDbContext<EmbcDbContext>(options => options
                    .EnableSensitiveDataLogging()
                    .UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ESS_develop;Integrated Security=True;"))
                ;

            serviceProvider = services.BuildServiceProvider();
        }
    }
}