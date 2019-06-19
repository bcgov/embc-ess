using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Services.Registrations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests.Registrations
{
    public class ViewRegistrationTests : TestBase
    {
        private IDataInterface di => Services.ServiceProvider.GetService<IDataInterface>();
        private EmbcDbContext db => Services.ServiceProvider.GetService<EmbcDbContext>();
        private IMediator mediator => Services.ServiceProvider.GetService<IMediator>();

        public ViewRegistrationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Get_SelfRegistrationWithNoReason_Success()
        {
            var selfReg = RegistrationGenerator.GenerateSelf();
            var regId = await di.CreateEvacueeRegistrationAsync(selfReg);

            var response = await mediator.Send(new RegistrationQueryRequest(regId, null));

            Assert.Null(response.FailureReason);
            Assert.Equal(RegistrationQueryResponse.ResponseStatus.Success, response.Status);
            Assert.NotNull(response.Registration);
        }

        [Fact]
        public async Task Get_CompleteRegistrationWithReason_Success()
        {
            var task = await di.CreateIncidentTaskAsync(IncidentTaskGenerator.Generate());
            var hostCommunity = (await di.GetCommunitiesAsync()).First();
            var completedReg = RegistrationGenerator.GenerateCompleted(task.Id, hostCommunity.Id);
            var regId = await di.CreateEvacueeRegistrationAsync(completedReg);

            var response = await mediator.Send(new RegistrationQueryRequest(regId, "want to read"));

            Assert.Null(response.FailureReason);
            Assert.Equal(RegistrationQueryResponse.ResponseStatus.Success, response.Status);
            Assert.NotNull(response.Registration);
        }

        [Fact]
        public async Task Get_CompleteRegistrationWithNoReason_Error()
        {
            var task = await di.CreateIncidentTaskAsync(IncidentTaskGenerator.Generate());
            var hostCommunity = (await di.GetCommunitiesAsync()).First();
            var completedReg = RegistrationGenerator.GenerateCompleted(task.Id, hostCommunity.Id);
            var regId = await di.CreateEvacueeRegistrationAsync(completedReg);

            var response = await mediator.Send(new RegistrationQueryRequest(regId, null));

            Assert.NotNull(response.FailureReason);
            Assert.Equal(RegistrationQueryResponse.ResponseStatus.Error, response.Status);
            Assert.Null(response.Registration);
        }

        [Fact]
        public async Task Get_SelfRegistrationWithReason_Success()
        {
            var selfReg = RegistrationGenerator.GenerateSelf();
            var regId = await di.CreateEvacueeRegistrationAsync(selfReg);

            var response = await mediator.Send(new RegistrationQueryRequest(regId, null));

            Assert.Null(response.FailureReason);
            Assert.Equal(RegistrationQueryResponse.ResponseStatus.Success, response.Status);
            Assert.NotNull(response.Registration);
        }

        [Fact]
        public async Task Get_RegistrationDoesntExists_NotFound()
        {
            var selfReg = RegistrationGenerator.GenerateSelf();
            var regId = await di.CreateEvacueeRegistrationAsync(selfReg);

            var response = await mediator.Send(new RegistrationQueryRequest(regId + "123", null));

            Assert.NotNull(response.FailureReason);
            Assert.Equal(RegistrationQueryResponse.ResponseStatus.NotFound, response.Status);
            Assert.Null(response.Registration);
        }

        [Fact]
        public async Task Get_CompleteRegistrationWithReason_AuditCreated()
        {
            var task = await di.CreateIncidentTaskAsync(IncidentTaskGenerator.Generate());
            var hostCommunity = (await di.GetCommunitiesAsync()).First();
            var completedReg = RegistrationGenerator.GenerateCompleted(task.Id, hostCommunity.Id);
            var regId = await di.CreateEvacueeRegistrationAsync(completedReg);

            var response = await mediator.Send(new RegistrationQueryRequest(regId, "want to read"));

            var auditEntries = db.EvacueeRegistrationAudits.Where(a => a.EssFileNumber == long.Parse(regId)).ToArray();
            Assert.Single(auditEntries);
            Assert.Equal(typeof(RegistrationViewed).Name, auditEntries[0].Action);
        }

        [Fact]
        public async Task GetAudit_CompletedRegistrationWithSingleFullView_ViewsReturned()
        {
            var task = await di.CreateIncidentTaskAsync(IncidentTaskGenerator.Generate());
            var hostCommunity = (await di.GetCommunitiesAsync()).First();
            var completedReg = RegistrationGenerator.GenerateCompleted(task.Id, hostCommunity.Id);
            var regId = await di.CreateEvacueeRegistrationAsync(completedReg);

            const string reason = "want to read";
            await mediator.Send(new RegistrationQueryRequest(regId, reason));

            var result = await mediator.Send(new RegistrationAuditQueryRequest(long.Parse(regId)));

            var entry = result.First();
            Assert.Equal(reason, entry.Reason);
            Assert.Equal(regId, entry.EssFileNumber);
            Assert.Equal("System", entry.UserName);
            Assert.Equal(DateTime.Now, entry.DateViewed, TimeSpan.FromSeconds(1));
        }
    }
}