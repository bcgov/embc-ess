using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface : IDataInterface
    {
        private readonly EmbcDbContext db;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DataInterface(EmbcDbContext ctx, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            db = ctx;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }
    }
}
