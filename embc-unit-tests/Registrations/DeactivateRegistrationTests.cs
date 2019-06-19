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
    public class DeactivateRegistrationTests : TestBase
    {
        private IMediator mediator => Services.ServiceProvider.GetService<IMediator>();
        private EmbcDbContext db => Services.ServiceProvider.GetService<EmbcDbContext>();

        public DeactivateRegistrationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Deactivate_RegistrationNotFinalized_NotActive()
        {
            var initial = await mediator.Send(new CreateNewRegistrationCommand(RegistrationGenerator.GenerateSelf()));

            var deactivated = await mediator.Send(new DeactivateRegistrationCommand(initial.Id));
            Assert.True(deactivated);

            var result = await mediator.Send(new RegistrationQueryRequest(initial.Id, null));
            Assert.NotNull(result);
            Assert.False(result.Registration.Active);

            var audit = db.EvacueeRegistrationAudits.ToArray();

            Assert.Equal(new[] { "RegistrationCreated", "RegistrationDeactivated", "RegistrationViewed" }, audit.Select(a => a.Action));
        }
    }
}