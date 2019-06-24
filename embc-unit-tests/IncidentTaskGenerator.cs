using Gov.Jag.Embc.Public.ViewModels;
using System;

namespace embc_unit_tests
{
    public static class IncidentTaskGenerator
    {
        public static IncidentTask Generate()
        {
            return new IncidentTask
            {
                TaskNumber = "D0000001",
                Active = true,
                StartDate = DateTime.Now,
                Details = "Test Task"
            };
        }
    }
}