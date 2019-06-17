using Gov.Jag.Embc.Public.Services.Registrations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests.Registrations
{
    public class CreateRegistrationTests : TestBase
    {
        public CreateRegistrationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create_NewSelfRegistration_Created()
        {
            var result = await Mediator.Send(new CreateNewRegistrationCommand(RegistrationGenerator.GenerateSelf()));

            Assert.NotNull(result);

            var audit = EmbcDb.EvacueeRegistrationAudits.ToArray();

            Assert.Single(audit);
            Assert.Contains(result.DietaryNeedsDetails, audit[0].Content);
        }
    }
}