using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Services.Registrations;
using Gov.Jag.Embc.Public.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests.Registrations
{
    public class UpdateRegistrationTests : BaseTest
    {
        public UpdateRegistrationTests(ITestOutputHelper output) : base(output,
            (typeof(IEmailSender), typeof(EmailSender)),
            (typeof(IDataInterface), typeof(DataInterface)))
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

        [Fact]
        public async Task Update_RegistrationToFinalized_Updated()
        {
            var initial = await Mediator.Send(new CreateNewRegistrationCommand(RegistrationGenerator.GenerateSelf()));
            Assert.False(initial.IsFinalized);

            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = (await di.GetCommunitiesAsync()).First();
            var toCommunity = (await di.GetCommunitiesAsync()).Last();

            var incidentTask = await di.CreateIncidentTaskAsync(new Gov.Jag.Embc.Public.ViewModels.IncidentTask()
            {
                Active = true,
                Community = new Gov.Jag.Embc.Public.ViewModels.Community { Id = fromCommunity.Id }
            });

            initial.HostCommunity = toCommunity;
            initial.RegistrationCompletionDate = DateTime.Now;
            await Mediator.Send(new UpdateRegistrationCommand(initial));

            var result = await Mediator.Send(new RegistrationQueryRequest(initial.Id, "reason"));
            Assert.NotNull(result);
            Assert.Equal(RegistrationQueryResponse.ResponseStatus.Success, result.Status);
            Assert.True(result.Registration.IsFinalized);

            var audit = EmbcDb.EvacueeRegistrationAudits.ToArray();

            Assert.Equal(new[] { "RegistrationCreated", "RegistrationUpdated", "RegistrationFinalized", "RegistrationViewed" }, audit.Select(a => a.Action));
        }
    }
}