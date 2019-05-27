using AutoMapper;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface : IDataInterface
    {
        private readonly EmbcDbContext db;
        private readonly IMapper mapper;

        public DataInterface(EmbcDbContext ctx, IMapper mapper)
        {
            db = ctx;
            this.mapper = mapper;
        }
    }
}
