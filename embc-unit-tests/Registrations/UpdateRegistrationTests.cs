using Gov.Jag.Embc.Public.Services.Registrations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests.Registrations
{
    public class UpdateRegistrationTests : TestBase
    {
        public UpdateRegistrationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Update_RegistrationNotFinalized_Updated()
        {
            var initial = await Mediator.Send(new CreateNewRegistrationCommand(RegistrationGenerator.GenerateSelf()));
            initial.DietaryNeedsDetails = "some other details";
            await Mediator.Send(new UpdateRegistrationCommand(initial));

            var result = await Mediator.Send(new RegistrationQueryRequest(initial.Id, null));
            Assert.NotNull(result);
            Assert.Equal(RegistrationQueryResponse.ResponseStatus.Success, result.Status);
            Assert.Equal(initial.DietaryNeedsDetails, result.Registration.DietaryNeedsDetails);

            var audit = EmbcDb.EvacueeRegistrationAudits.ToArray();

            Assert.Equal(new[] { "RegistrationCreated", "RegistrationUpdated", "RegistrationViewed" }, audit.Select(a => a.Action));
        }
    }
}