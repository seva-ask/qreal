
namespace QReal.Web.Assemblies
{
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;

    [EnableClientAccess]
    public class AssembliesService : DomainService
    {
        [Invoke]
        public string[] GetAssemblies()
        {
            return AssemblyManager.Instance.Assemblies.Keys.ToArray();
        }

        [Invoke]
        public byte[] GetSerialiazedAssembly(string file)
        {
            return AssemblyManager.Instance.Assemblies[file];
        }
    }
}


