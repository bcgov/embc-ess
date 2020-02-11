using Gov.Jag.Embc.Public.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Utils
{
    public interface ICurrentUser
    {
        User CurrentUser { get; }
    }
}
