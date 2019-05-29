using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Services.Registrations;
using Gov.Jag.Embc.Public.Utils;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests.Registrations
{
    public class CreateRegistrationTests : BaseTest
    {
        public CreateRegistrationTests(ITestOutputHelper output) : base(output,
            (typeof(IEmailSender), typeof(EmailSender)),
            (typeof(IDataInterface), typeof(DataInterface)))
        {
        }

        [Fact]
        public async Task Create_NewSelfRegistration_Success()
        {
            var result = await Mediator.Send(new CreateNewRegistrationCommand(RegistrationGenerator.GenerateSelf()));

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_NewSelfRegistration_AuditCreated()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var registration = RegistrationGenerator.GenerateSelf();
            var result = await Mediator.Send(new CreateNewRegistrationCommand(registration));

            var audit = EmbcDb.EvacueeRegistrationAudits.ToArray();

            Assert.Single(audit);
            Assert.Contains(registration.DietaryNeedsDetails, audit[0].Content);
        }
    }
}