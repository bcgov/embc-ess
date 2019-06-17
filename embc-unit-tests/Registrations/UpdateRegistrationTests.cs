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
    public class UpdateRegistrationTests : TestBase
    {
        private IMediator mediator => Services.ServiceProvider.GetService<IMediator>();
        private EmbcDbContext db => Services.ServiceProvider.GetService<EmbcDbContext>();

        public UpdateRegistrationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Update_RegistrationNotFinalized_Updated()
        {
            var initial = await mediator.Send(new CreateNewRegistrationCommand(RegistrationGenerator.GenerateSelf()));
            initial.DietaryNeedsDetails = "some other details";
            await mediator.Send(new UpdateRegistrationCommand(initial));

            var result = await mediator.Send(new RegistrationQueryRequest(initial.Id, null));
            Assert.NotNull(result);
            Assert.Equal(RegistrationQueryResponse.ResponseStatus.Success, result.Status);
            Assert.Equal(initial.DietaryNeedsDetails, result.Registration.DietaryNeedsDetails);

            var audit = db.EvacueeRegistrationAudits.ToArray();

            Assert.Equal(new[] { "RegistrationCreated", "RegistrationUpdated", "RegistrationViewed" }, audit.Select(a => a.Action));
        }
    }
}