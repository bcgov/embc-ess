using AutoMapper;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class RegistrationQueryService :
        IRequestHandler<RegistrationQueryRequest, RegistrationQueryResponse>,
        IRequestHandler<RegistrationSummaryQueryRequest, ViewModels.RegistrationSummary>,
        IRequestHandler<RegistrationAuditQueryRequest, IEnumerable<RegistrationViewEntry>>
    {
        private readonly IDataInterface dataInterface;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public RegistrationQueryService(IDataInterface dataInterface, IMediator mediator, IMapper mapper)
        {
            this.dataInterface = dataInterface;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<RegistrationQueryResponse> Handle(RegistrationQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await dataInterface.GetEvacueeRegistrationAsync(request.EssFileNumber);
            RegistrationQueryResponse response;
            if (result != null && result.IsFinalized && string.IsNullOrWhiteSpace(request.Reason))
                response = RegistrationQueryResponse.Error($"Must specify a valid reason to view the complete registration {request.EssFileNumber}");
            else if (result == null)
                response = RegistrationQueryResponse.NotFound(request.EssFileNumber);
            else
            {
                response = RegistrationQueryResponse.Success(result);
                await mediator.Publish(new RegistrationViewed(request.EssFileNumber, request.Reason));
            }

            return response;
        }

        public async Task<ViewModels.RegistrationSummary> Handle(RegistrationSummaryQueryRequest request, CancellationToken cancellationToken)
        {
            return await dataInterface.GetEvacueeRegistrationSummaryAsync(request.EssFileNumber);
        }

        public async Task<IEnumerable<RegistrationViewEntry>> Handle(RegistrationAuditQueryRequest request, CancellationToken cancellationToken)
        {
            var auditTrail = await dataInterface.GetEvacueeRegistrationAuditTrailAsync(request.EssFileNumber);

            return auditTrail
                .Where(a => a.Action == typeof(RegistrationViewed).Name)
                .Select(a => mapper.Map<RegistrationViewEntry>(a))
                .Where(a => !string.IsNullOrEmpty(a.Reason));
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

    public class RegistrationViewed : RegistrationEvent
    {
        public string ReasonForView { get; }

        public RegistrationViewed(string essFileNumber, string reasonForView) : base(essFileNumber)
        {
            ReasonForView = reasonForView;
        }
    }

    public class RegistrationAuditQueryRequest : IRequest<IEnumerable<RegistrationViewEntry>>
    {
        public RegistrationAuditQueryRequest(long essFileNumber)
        {
            EssFileNumber = essFileNumber;
        }

        public long EssFileNumber { get; }
    }

    public class RegistrationViewEntry
    {
        public string EssFileNumber { get; set; }
        public string UserName { get; set; }
        public DateTime DateViewed { get; set; }
        public string Reason { get; set; }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EvacueeRegistrationAudit, RegistrationViewEntry>()
                .ForMember(d => d.DateViewed, opts => opts.MapFrom(s => s.Date.DateTime))
                .ForMember(d => d.Reason, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<RegistrationViewed>(s.Content).ReasonForView))
                .ReverseMap()
                .ForMember(d => d.Date, opts => opts.MapFrom(s => new DateTimeOffset(s.DateViewed)))
                ;
        }
    }
}
