
using Gov.Jag.Embc.Public.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Utils
{
    public class IncidentTaskHelper
    {
        /// <summary>
        /// Validates that an Incident Task from the client has valid properties.
        /// </summary>
        /// <param name="item">The IncidentTask to validate.</param>
        /// <returns>
        /// Return a Tuple with two items, Item1 is the name of the property with an error and item 2 is the error details.
        /// Returns null when no errors are found.
        /// </returns>
        public static Tuple<string, string> ValidateClientTaskProperties(IncidentTask item)
        {
            Tuple<string, string> results = null;

            if (item.StartDate.HasValue && item.StartDate.Value.ToUniversalTime() > DateTime.UtcNow)
            {
                results = new Tuple<string, string>("StartDate", "Incident start date cannot be in the future");
            }
            else if (!item.TaskNumberStartDate.HasValue)
            {
               results = new Tuple<string, string>("TaskNumberStartDate", "Incident task must have a task number start date");
            }
            else if (item.TaskNumberStartDate.HasValue && item.TaskNumberStartDate.Value.ToUniversalTime() > DateTime.UtcNow)
            {
                results = new Tuple<string, string>("TaskNumberStartDate", "Incident task number start date cannot be in the future");
            }
            else if (!item.TaskNumberEndDate.HasValue)
            {
                results = new Tuple<string, string>("TaskNumberEndDate", "Incident task must have a task number end date");
            }

            return results;
        }
    }
}
