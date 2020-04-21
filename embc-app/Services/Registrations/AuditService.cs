using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public abstract class RegistrationEvent : INotification
    {
        protected RegistrationEvent(string essFileNumber)
        {
            EssFileNumber = essFileNumber;
        }

        public string EssFileNumber { get; }
    }

    //Asp.Net Core DI doesn't support covariant resolution, need to register
    // individual handlers instead of using polymorphic notification handling
    //https://github.com/jbogard/MediatR/wiki/Container-Feature-Support
    public class AuditService :
        INotificationHandler<RegistrationViewed>,
        INotificationHandler<RegistrationDeactivated>,
        INotificationHandler<RegistrationUpdated>,
        INotificationHandler<RegistrationFinalized>,
        INotificationHandler<RegistrationCreated>
    {
        private readonly IDataInterface dataInterface;
        private readonly IHttpContextAccessor httpContext;

        public AuditService(IDataInterface dataInterface, IHttpContextAccessor httpContext)
        {
            this.dataInterface = dataInterface;
            this.httpContext = httpContext;
        }

        private async Task Handle(RegistrationEvent notification)
        {
            var user = httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid) ?? "System";
            var userName = httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.Upn) ?? "System";
            var userType = httpContext.HttpContext?.User?.FindFirstValue(EssClaimTypes.USER_TYPE) ?? "System";
            await dataInterface.AppendEvacueeRegistrationAuditEntryAsync(notification, user, userName, userType);
        }

        public async Task Handle(RegistrationViewed notification, CancellationToken cancellationToken)
        {
            await Handle(notification);
        }

        public async Task Handle(RegistrationDeactivated notification, CancellationToken cancellationToken)
        {
            await Handle(notification);
        }

        public async Task Handle(RegistrationUpdated notification, CancellationToken cancellationToken)
        {
            await Handle(notification);
        }

        public async Task Handle(RegistrationFinalized notification, CancellationToken cancellationToken)
        {
            await Handle(notification);
        }

        public async Task Handle(RegistrationCreated notification, CancellationToken cancellationToken)
        {
            await Handle(notification);
        }
    }
}
