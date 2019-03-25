namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface : IDataInterface
    {
        private readonly EmbcDbContext db;

        public DataInterface(EmbcDbContext ctx)
        {
            db = ctx;
        }
    }
}
