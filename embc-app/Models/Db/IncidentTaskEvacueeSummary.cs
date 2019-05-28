using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class IncidentTaskEvacueeSummary
    {
        [ForeignKey("IncidentTask")]
        public Guid IncidentTaskId { get; set; }

        public string TaskNumber { get; set; }
        public int TotalEvacuees { get; set; }
        public IncidentTask IncidentTask { get; set; }
    }
}
