
namespace QReal.Web.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // Implements application logic using the QRealEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class InstancesService : LinqToEntitiesDomainService<QRealEntities>
    {

        // TODO: Consider
        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<Instance> GetInstances()
        {
            return this.ObjectContext.Instances;
        }

        public void InsertInstance(Instance instance)
        {
            if ((instance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(instance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Instances.AddObject(instance);
            }
        }

        public void UpdateInstance(Instance currentInstance)
        {
            this.ObjectContext.Instances.AttachAsModified(currentInstance, this.ChangeSet.GetOriginal(currentInstance));
        }

        public void DeleteInstance(Instance instance)
        {
            if ((instance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Instances.Attach(instance);
            }
            this.ObjectContext.Instances.DeleteObject(instance);
        }
    }
}


