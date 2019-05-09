using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
            : this("MyProfile")
        {
        }

        protected AutoMapperProfileConfiguration(string profileName)
            : base(profileName)
        {
            CreateMap<Models.Db.Community, ViewModels.Community>()
                .ForMember(x => x.Id, x => x.MapFrom(opt => opt.Id.ToString()));

            CreateMap<Models.Db.Region, ViewModels.Region>()
                .ForMember(x => x.Id, x => x.MapFrom(opt => opt.Name));
        }
    }
}
