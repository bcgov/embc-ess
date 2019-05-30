using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using MediatR;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public abstract class RegistrationEvent : INotification
    {
        public RegistrationEvent(string essFileNumber)
        {
            EssFileNumber = essFileNumber;
        }

        public string EssFileNumber { get; }
    }

    //public class RegistrationCreated : RegistrationEvent
    //{
    //    public RegistrationCreated(string essFileNumber, ViewModels.Registration registration) : base(essFileNumber)
    //    {
    //        Registration = registration;
    //    }

    //    public ViewModels.Registration Registration { get; }
    //}

    //public class RegistrationFinalized : RegistrationEvent
    //{
    //    public RegistrationFinalized(string essFileNumber, ViewModels.Registration registration) : base(essFileNumber)
    //    {
    //        Registration = registration;
    //    }

    //    public ViewModels.Registration Registration { get; }
    //}

    //public class RegistrationUpdated : RegistrationEvent
    //{
    //    public ViewModels.Registration Registration { get; }

    //    public RegistrationUpdated(string essFileNumber, ViewModels.Registration registration) : base(essFileNumber)
    //    {
    //        Registration = registration;
    //    }
    //}

    //public class RegistrationDeactivated : RegistrationEvent
    //{
    //    public RegistrationDeactivated(string essFileNumber) : base(essFileNumber)
    //    {
    //    }
    //}

    public class RegistrationEventStoreHandler : INotificationHandler<RegistrationViewed>
    {
        private readonly IDataInterface dataInterface;

        public RegistrationEventStoreHandler(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        private async Task Handle(RegistrationEvent notification)
        {
            var user = ClaimsPrincipal.Current?.FindFirstValue(EssClaimTypes.USER_ID) ?? "System";
            await dataInterface.AppendEvacueeRegistrationAuditEntryAsync(notification, user);
        }

        public async Task Handle(RegistrationViewed notification, CancellationToken cancellationToken)
        {
            await Handle(notification);
        }
    }
}
