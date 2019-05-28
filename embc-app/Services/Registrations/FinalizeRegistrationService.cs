using Gov.Jag.Embc.Public.DataInterfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class FinalizeRegistrationService : AsyncRequestHandler<FinalizeRegistrationCommand>
    {
        private readonly IDataInterface dataInterface;

        public FinalizeRegistrationService(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        protected async override Task Handle(FinalizeRegistrationCommand request, CancellationToken cancellationToken)
        {
            await dataInterface.UpdateEvacueeRegistrationAsync(request.Registration);
        }
    }

    public class FinalizeRegistrationCommand : IRequest
    {
        public FinalizeRegistrationCommand(ViewModels.Registration registration)
        {
            Registration = registration;
        }

        public ViewModels.Registration Registration { get; }
    }
}
