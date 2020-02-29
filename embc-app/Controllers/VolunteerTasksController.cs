using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/volunteer-task")]
    [Authorize]
    public class VolunteerTasksController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger logger;
        private readonly IHostingEnvironment env;
        private readonly IDataInterface dataInterface;

        public VolunteerTasksController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IHostingEnvironment env, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            this.configuration = configuration;

            this.httpContextAccessor = httpContextAccessor;
            logger = loggerFactory.CreateLogger(typeof(VolunteerTasksController));
            this.env = env;
        }


        [HttpPost("task/{id}")]
        public async Task<IActionResult> SetVolunteerTask(string id)
        {
            // get incident task
            var task = await dataInterface.GetIncidentTaskByTaskNumbetAsync(id);
            if (task == null) return NotFound(Json(id));

            // get volunteerTask by incident task id
            var volunteerTask = await dataInterface.GetVolunteerTaskByIncideTaskIdAsync(Guid.Parse(task.Id));

            //if volunteerTask does not exist, create it
            var volunteerId = HttpContext.User.FindFirstValue(EssClaimTypes.USER_ID);

            if (volunteerTask == null)
            {
                var newVolunteerTask = new VolunteerTask()
                {
                    IncidentTaskId = Guid.Parse(task.Id),
                    VolunteerId = int.Parse(volunteerId),
                    LastDateVolunteerConfirmedTask = DateTime.Now
                };
                volunteerTask = await dataInterface.CreateVolunteerTaskAsync(newVolunteerTask);
            }
            // otherwise update the task
            else
            {
                volunteerTask.LastDateVolunteerConfirmedTask = DateTime.Now;
                volunteerTask.VolunteerId = int.Parse(volunteerId);
                await dataInterface.UpdateVolunteerTasksAsync(volunteerTask);
            }

            return Json(volunteerTask);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VolunteerTask item)
        {
            // var existing = await dataInterface.GetVolunteerByBceidUserNameAsync(item.BceidAccountNumber);
            // if (existing != null)
            // {
            //     ModelState.AddModelError("Externaluseridentifier", $"Duplicate Id {item.Id} found.");
            // }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await dataInterface.CreateVolunteerTaskAsync(item);
            return Json(await dataInterface.GetVolunteerTaskByIdAsync(result.Id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Volunteer item, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id)
            {
                return BadRequest(Json(id));
            }

            var existing = await dataInterface.GetVolunteerTaskByIdAsync(int.Parse(id));
            if (existing == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataInterface.UpdateVolunteerAsync(item);
            return Ok();
        }
    }
}
