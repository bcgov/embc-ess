using Gov.Jag.Embc.Public.DataInterfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class DeactivateRegistrationService : IRequestHandler<DeactivateRegistrationCommand, bool>
    {
        private readonly IDataInterface dataInterface;

        public DeactivateRegistrationService(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        public async Task<bool> Handle(DeactivateRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await dataInterface.DeactivateEvacueeRegistrationAsync(request.EssFileNumber);
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
}
