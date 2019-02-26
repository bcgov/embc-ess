using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{

    

    public class Person
    {
        public string id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string title { get; set; }

        public string phoneNumber { get; set; }

        public string phoneNumberAlt { get; set; }

        public string email { get; set; }

        public bool? isOwner { get; set; }

    }
}
