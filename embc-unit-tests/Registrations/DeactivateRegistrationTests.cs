using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Services.Registrations;
using Gov.Jag.Embc.Public.Utils;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests.Registrations
{
    public class DeactivateRegistrationTests : TestBase
    {
        public DeactivateRegistrationTests(ITestOutputHelper output) : base(output,
            (typeof(IEmailSender), typeof(EmailSender)),
            (typeof(IDataInterface), typeof(DataInterface)))
        {
        }

        [Fact]
        public async Task Deactivate_RegistrationNotFinalized_NotActive()
        {
            var initial = await Mediator.Send(new CreateNewRegistrationCommand(RegistrationGenerator.GenerateSelf()));

            var deactivated = await Mediator.Send(new DeactivateRegistrationCommand(initial.Id));
            Assert.True(deactivated);

            var result = await Mediator.Send(new RegistrationQueryRequest(initial.Id, null));
            Assert.NotNull(result);
            Assert.False(result.Registration.Active);

            var audit = EmbcDb.EvacueeRegistrationAudits.ToArray();

            Assert.Equal(new[] { "RegistrationCreated", "RegistrationDeactivated", "RegistrationViewed" }, audit.Select(a => a.Action));
        }
    }
}