using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public class ReferralsService
    {
        //Restaurant Meals
        //GROCERIES
        //Hotel/Motel
        //Group Lodging
        //Billeting
        //Clothing
        //Taxi
        //Incidentals

        public string GetHtmlContent()
        {
            var data = new
            {
                title = "My new post",
                body = "This is my first post!"
            };

            var handleBars = Handlebars.Create(new HandlebarsConfiguration { FileSystem = new DiskFileSystem() });

            var template = handleBars.CompileView("Services/Referrals/Views/test.hbs");
            var result = template(data);

            return result;
        }
    }
}
