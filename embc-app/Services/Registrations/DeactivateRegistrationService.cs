using Gov.Jag.Embc.Public.DataInterfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class DeactivateRegistrationService : IRequestHandler<DeactivateRegistrationCommand, bool>
    {
        private readonly IDataInterface dataInterface;
        private readonly IMediator mediator;

        public DeactivateRegistrationService(IDataInterface dataInterface, IMediator mediator)
        {
            this.dataInterface = dataInterface;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(DeactivateRegistrationCommand request, CancellationToken cancellationToken)
        {
            var result = await dataInterface.DeactivateEvacueeRegistrationAsync(request.EssFileNumber);
            if (result) await mediator.Publish(new RegistrationDeactivated(request.EssFileNumber));

            return result;
        }
    }

    public class DeactivateRegistrationCommand : IRequest<bool>
    {
        public DeactivateRegistrationCommand(string essFileNumber)
        {
            EssFileNumber = essFileNumber;
        }

        public string EssFileNumber { get; }
    }

    public class RegistrationDeactivated : RegistrationEvent
    {
        public RegistrationDeactivated(string essFileNumber) : base(essFileNumber)
        {
        }
    }
}
