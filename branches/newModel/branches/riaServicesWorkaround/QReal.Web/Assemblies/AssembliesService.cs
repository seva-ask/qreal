
namespace QReal.Web.Assemblies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
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


