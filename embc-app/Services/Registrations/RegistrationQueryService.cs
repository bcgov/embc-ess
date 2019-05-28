using Gov.Jag.Embc.Public.DataInterfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class RegistrationQueryService :
        IRequestHandler<RegistrationQueryRequest, RegistrationQueryResponse>,
        IRequestHandler<RegistrationSummaryQueryRequest, ViewModels.RegistrationSummary>
    {
        private readonly IDataInterface dataInterface;

        public RegistrationQueryService(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        public async Task<RegistrationQueryResponse> Handle(RegistrationQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await dataInterface.GetEvacueeRegistrationAsync(request.EssFileNumber);
            if (result != null && result.IsFinalized && string.IsNullOrWhiteSpace(request.Reason))
                return RegistrationQueryResponse.Error($"Must specify a valid reason to view the complete registration {request.EssFileNumber}");
            if (result == null)
                return RegistrationQueryResponse.NotFound(request.EssFileNumber);
            return RegistrationQueryResponse.Success(result);
        }

        public async Task<ViewModels.RegistrationSummary> Handle(RegistrationSummaryQueryRequest request, CancellationToken cancellationToken)
        {
            return await dataInterface.GetEvacueeRegistrationSummaryAsync(request.EssFileNumber);
        }
    }

    public class RegistrationQueryRequest : IRequest<RegistrationQueryResponse>
    {
        public RegistrationQueryRequest(string essFileNumber, string reason)
        {
            EssFileNumber = essFileNumber;
            Reason = reason;
        }

        public string EssFileNumber { get; }
        public string Reason { get; }
    }

    public class RegistrationQueryResponse
    {
        public enum ResponseStatus
        {
            Success = 1,
            NotFound,
            Error
        }

        public static RegistrationQueryResponse Success(ViewModels.Registration registration)
        {
            return new RegistrationQueryResponse
            {
                Status = ResponseStatus.Success,
                Registration = registration
            };
        }

        public static RegistrationQueryResponse NotFound(string essFileNumber)
        {
            return new RegistrationQueryResponse
            {
                Status = ResponseStatus.NotFound,
                FailureReason = $"Registration with ESS File Number '{essFileNumber}' not found"
            };
        }

        public static RegistrationQueryResponse Error(string reason)
        {
            return new RegistrationQueryResponse
            {
                Status = ResponseStatus.Error,
                FailureReason = reason
            };
        }

        public ViewModels.Registration Registration { get; private set; }

        public string FailureReason { get; private set; }
        public ResponseStatus Status { get; private set; }
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
