
namespace QReal.Web.Database
{
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;

    // Implements application logic using the QRealEntities context.
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess]
    public class InstancesService : LinqToEntitiesDomainService<QRealEntities>
    {

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<GraphicInstance> GetGraphicInstances()
        {
            return this.ObjectContext.GraphicInstances;
        }

        public void InsertGraphicInstance(GraphicInstance graphicInstance)
        {
            if ((graphicInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(graphicInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.GraphicInstances.AddObject(graphicInstance);
            }
        }

        public void UpdateGraphicInstance(GraphicInstance currentGraphicInstance)
        {
            this.ObjectContext.GraphicInstances.AttachAsModified(currentGraphicInstance, this.ChangeSet.GetOriginal(currentGraphicInstance));
        }

        public void DeleteGraphicInstance(GraphicInstance graphicInstance)
        {
            if ((graphicInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.GraphicInstances.Attach(graphicInstance);
            }
            this.ObjectContext.GraphicInstances.DeleteObject(graphicInstance);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<InstanceProperty> GetInstanceProperties()
        {
            return this.ObjectContext.InstanceProperties;
        }

        public void InsertInstanceProperty(InstanceProperty instanceProperty)
        {
            if ((instanceProperty.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(instanceProperty, EntityState.Added);
            }
            else
            {
                this.ObjectContext.InstanceProperties.AddObject(instanceProperty);
            }
        }

        public void UpdateInstanceProperty(InstanceProperty currentInstanceProperty)
        {
            this.ObjectContext.InstanceProperties.AttachAsModified(currentInstanceProperty, this.ChangeSet.GetOriginal(currentInstanceProperty));
        }

        public void DeleteInstanceProperty(InstanceProperty instanceProperty)
        {
            if ((instanceProperty.EntityState == EntityState.Detached))
            {
                this.ObjectContext.InstanceProperties.Attach(instanceProperty);
            }
            this.ObjectContext.InstanceProperties.DeleteObject(instanceProperty);
        }

        // 1. Adding parameters to this method and constraining returned results, and/or
        // 2. Adding query methods taking different parameters.
        public IQueryable<LogicalInstance> GetLogicalInstances()
        {
            return this.ObjectContext.LogicalInstances;
        }

        public void InsertLogicalInstance(LogicalInstance logicalInstance)
        {
            if ((logicalInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(logicalInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.LogicalInstances.AddObject(logicalInstance);
            }
        }

        public void UpdateLogicalInstance(LogicalInstance currentLogicalInstance)
        {
            this.ObjectContext.LogicalInstances.AttachAsModified(currentLogicalInstance, this.ChangeSet.GetOriginal(currentLogicalInstance));
        }

        public void DeleteLogicalInstance(LogicalInstance logicalInstance)
        {
            if ((logicalInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.LogicalInstances.Attach(logicalInstance);
            }
            this.ObjectContext.LogicalInstances.DeleteObject(logicalInstance);
        }
    }
}


