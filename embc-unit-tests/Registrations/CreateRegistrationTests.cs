using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Services.Registrations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests.Registrations
{
    public class CreateRegistrationTests : TestBase
    {
        private IMediator mediator => Services.ServiceProvider.GetService<IMediator>();
        private EmbcDbContext db => Services.ServiceProvider.GetService<EmbcDbContext>();

        public CreateRegistrationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create_NewSelfRegistration_Created()
        {
            var result = await mediator.Send(new CreateNewRegistrationCommand(RegistrationGenerator.GenerateSelf()));

            Assert.NotNull(result);

            var audit = db.EvacueeRegistrationAudits.ToArray();

            Assert.Single(audit);
            Assert.Contains(result.DietaryNeedsDetails, audit[0].Content);
        }
    }
}