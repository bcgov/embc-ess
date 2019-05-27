using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class RegistrationQueryService :
        IRequestHandler<RegistrationQueryRequest, ViewModels.Registration>,
        IRequestHandler<RegistrationSummaryQueryRequest, ViewModels.RegistrationSummary>
    {
        private readonly IDataInterface dataInterface;

        public RegistrationQueryService(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        public async Task<Registration> Handle(RegistrationQueryRequest request, CancellationToken cancellationToken)
        {
            return await dataInterface.GetEvacueeRegistrationAsync(request.EssFileNumber);
        }

        public async Task<RegistrationSummary> Handle(RegistrationSummaryQueryRequest request, CancellationToken cancellationToken)
        {
            return await dataInterface.GetEvacueeRegistrationSummaryAsync(request.EssFileNumber);
        }
    }

    public class RegistrationQueryRequest : IRequest<ViewModels.Registration>
    {
        public RegistrationQueryRequest(string essFileNumber)
        {
            EssFileNumber = essFileNumber;
        }

        public string EssFileNumber { get; }
    }

    public class RegistrationSummaryQueryRequest : IRequest<ViewModels.RegistrationSummary>
    {
        public RegistrationSummaryQueryRequest(string essFileNumber)
        {
            EssFileNumber = essFileNumber;
        }

        public string EssFileNumber { get; }
    }
}
