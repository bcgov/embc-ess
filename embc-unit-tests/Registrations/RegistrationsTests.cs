//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;

//namespace embc_unit_tests.Registrations
//{
//    public class RegistrationsTests
//    {
//        [Theory]
//        [MemberData(nameof(GetRegistrations)]
//        public async Task Create_Registration_Saved(Gov.Jag.Embc.Public.ViewModels.Registration registration)
//        {
//            //var result = await Mediator.Send(new CreateNewRegistrationCommand(registration));

//            //Assert.NotNull(result);
//        }

//        public static IEnumerable<object[]> GetRegistrations(TestBase testContext)
//        {
//            var incidentTask = IncidentTaskGenerator.Generate();
//        }
//    }
//}