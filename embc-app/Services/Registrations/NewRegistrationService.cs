using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class NewRegistrationService : IRequestHandler<CreateNewRegistrationCommand, ViewModels.Registration>
    {
        private readonly IDataInterface dataInterface;
        private readonly IEmailSender emailSender;
        private readonly IMediator mediator;

        public NewRegistrationService(IDataInterface dataInterface, IEmailSender emailSender, IMediator mediator)
        {
            this.dataInterface = dataInterface;
            this.emailSender = emailSender;
            this.mediator = mediator;
        }

        public async Task<ViewModels.Registration> Handle(CreateNewRegistrationCommand request, CancellationToken cancellationToken)
        {
            var registration = request.Registration;
            registration.Id = null;
            registration.Active = true;
            var essFileNumber = await dataInterface.CreateEvacueeRegistrationAsync(registration);
            var result = await dataInterface.GetEvacueeRegistrationAsync(essFileNumber.ToString());
            await mediator.Publish(new RegistrationCreated(essFileNumber.ToString(), result));
            if (result.IsFinalized) await mediator.Publish(new RegistrationFinalized(essFileNumber.ToString(), result));

            //TODO: move to a notification handler
            if (!string.IsNullOrWhiteSpace(request.Registration.HeadOfHousehold.Email))
            {
                var registrationEmail = CreateEmailMessageForRegistration(result);
                emailSender.Send(registrationEmail);
            }
            return result;
        }

        private EmailMessage CreateEmailMessageForRegistration(ViewModels.Registration registration)
        {
            //var essRegistrationLink = @"<a target='_blank' href='https://justice.gov.bc.ca/embcess/'>Evacuee Self-Registration</a>";
            var emergencyInfoBCLink = @"<a target='_blank' href='https://www.emergencyinfobc.gov.bc.ca/'>Emergency Info BC</a>";

            var subject = "Registration completed successfully";
            var body = $@"
                <p><h1 style=""color: darkblue"">Submission Complete</h1></p>
                <p></p>
                <p><h2 style=""color: blue"">Your Emergency Support Services File Number is: <strong>{registration.EssFileNumber}</strong></h2></p>
                <p></p>
                <p>Thank you for submitting your online self-registration.</p>
                ";

            // var body = "<h2>Evacuee Registration Success</h2><br/>" + "<b>What you need to know:</b><br/><br/>" +
            //    $"Your Emergency Support Services File Number is: <b>{registration.EssFileNumber}</b>";

            if (registration.IncidentTask == null)
            {
                body += $@"
                <h3 style=""color: blue""><strong>Next Steps</strong></h3>
                <ul>
                    <li>Please keep a record of your Emergency Support Services File Number to receive emergency support services that can be provided up to 72 hours starting from the time connecting in with a local ESS Responder at a Reception Centre.</li>
                    <li>After a need's assessment interview with a local ESS Responder has been completed, supports are provided to purchase goods and services if eligible.</li>
                    <li>Any goods and services purchased prior to a need's assessment interview are not eligible for retroactive reimbursement.</li>
                    <li>If you are under<strong>EVACUATION ALERT</strong> or<strong> DO NOT</strong> require emergency serves at this time, no further action is required.</li>
                    <li>If you are under <strong>EVACUATION ORDER</strong>, and require emergency supports, proceed to your nearest Reception Centre. A list of open Reception Centres can be found at {emergencyInfoBCLink}.</li>
                    <li>If<strong> NO</strong> nearby Reception Centre is open and immediate action is required, please contact your Local Emergency Program for next steps.</li>
                </ul>
                ";
            }

            return new EmailMessage(registration.HeadOfHousehold.Email, subject, body);
        }
    }

    public class CreateNewRegistrationCommand : IRequest<ViewModels.Registration>
    {
        public CreateNewRegistrationCommand(ViewModels.Registration registration)
        {
            Registration = registration;
        }

        public ViewModels.Registration Registration { get; }
    }

    public class RegistrationCreated : RegistrationEvent
    {
        public RegistrationCreated(string essFileNumber, ViewModels.Registration registration) : base(essFileNumber)
        {
            Registration = registration;
        }

        public ViewModels.Registration Registration { get; }
    }

    public class RegistrationFinalized : RegistrationEvent
    {
        public RegistrationFinalized(string essFileNumber, ViewModels.Registration registration) : base(essFileNumber)
        {
            Registration = registration;
        }

        public ViewModels.Registration Registration { get; }
    }
}
