using AutoMapper;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class ViewModelConversions
    {
        private static IMapper mapper => Mapper.Instance;
    }
}
