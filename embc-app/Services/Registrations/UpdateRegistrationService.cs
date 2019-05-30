using Gov.Jag.Embc.Public.DataInterfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class UpdateRegistrationService : AsyncRequestHandler<UpdateRegistrationCommand>
    {
        private readonly IDataInterface dataInterface;
        private readonly IMediator mediator;

        public UpdateRegistrationService(IDataInterface dataInterface, IMediator mediator)
        {
            this.dataInterface = dataInterface;
            this.mediator = mediator;
        }

        protected async override Task Handle(UpdateRegistrationCommand request, CancellationToken cancellationToken)
        {
            var essFileNumber = request.Registration.EssFileNumber.Value;
            var before = await dataInterface.GetEvacueeRegistrationAsync(essFileNumber.ToString());
            await dataInterface.UpdateEvacueeRegistrationAsync(request.Registration);
            var after = await dataInterface.GetEvacueeRegistrationAsync(essFileNumber.ToString());
            await mediator.Publish(new RegistrationUpdated(essFileNumber.ToString(), request.Registration));
            if (!before.IsFinalized && after.IsFinalized) await mediator.Publish(new RegistrationFinalized(essFileNumber.ToString(), request.Registration));
        }
    }

    public class UpdateRegistrationCommand : IRequest
    {
        public UpdateRegistrationCommand(ViewModels.Registration registration)
        {
            Registration = registration;
        }

        public ViewModels.Registration Registration { get; }
    }

    public class RegistrationUpdated : RegistrationEvent
    {
        public ViewModels.Registration Registration { get; }

        public RegistrationUpdated(string essFileNumber, ViewModels.Registration registration) : base(essFileNumber)
        {
            Registration = registration;
        }
    }
}
